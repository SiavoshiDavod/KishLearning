using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

public sealed class TextRun
{
    public string Text { get; set; }
    public Color Color { get; set; }

    public TextRun(string text, Color color)
    {
        Text = text ?? string.Empty;
        Color = color;
    }
}

public static class ImageTextPrinterColorBiz
{
    public static byte[] DrawTextOnImage(
        byte[] imageBytes,
        string text,
        string fontPath,
        string fontName,
        float initialFontSize,
        int maxWidth,
        int maxHeight,
        int x,
        int y,
        float lineSpacingPx,
        Color? defaultColor = null)
    {
        defaultColor = Color.Black;

        using (var ms = new MemoryStream(imageBytes))
        using (var img = Image.FromStream(ms))
        using (var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            g.DrawImage(img, 0, 0, img.Width, img.Height);

            Font font = BuildFont(fontPath, fontName, initialFontSize);

            // 1) پارس تگ‌های رنگی
            var runs = ParseColoredRuns(text ?? string.Empty, defaultColor.Value);

            // 2) تبدیل به خطوط خام بر اساس \n (حفظ خطوط خالی)
            var rawLines = SplitRunsByNewline(runs);

            // 3) شکست دستی خطوط بر اساس عرض
            var format = new StringFormat(StringFormatFlags.DirectionRightToLeft
                                          | StringFormatFlags.NoWrap
                                          | StringFormatFlags.MeasureTrailingSpaces);

            var finalLines = new List<List<TextRun>>();
            foreach (var lineRuns in rawLines)
            {
                finalLines.AddRange(BreakLineRunsByWidth(g, lineRuns, font, maxWidth, format));
            }

            // 4) محدودیت ارتفاع
            float lineHeight = MeasureLineHeight(g, font, format);
            float lineAdvance = lineHeight + lineSpacingPx;
            int maxLines = (int)Math.Floor(maxHeight / lineAdvance);

            if (finalLines.Count > maxLines)
                finalLines = finalLines.Take(Math.Max(0, maxLines)).ToList();

            // 5) رسم خطوط
            float yCursor = y;
            foreach (var lineRuns in finalLines)
            {
                var rect = new RectangleF(x, yCursor, maxWidth, lineHeight);
                DrawColoredRunsOnSingleLine(g, lineRuns, font, rect, rtl: true);
                yCursor += lineAdvance;

                if (yCursor > y + maxHeight) break;
            }

            using (var outMs = new MemoryStream())
            {
                bmp.Save(outMs, img.RawFormat);
                return outMs.ToArray();
            }
        }
    }

    private static Font BuildFont(string fontPath, string fontName, float size)
    {
        if (!string.IsNullOrWhiteSpace(fontPath) && File.Exists(fontPath))
        {
            var pfc = new PrivateFontCollection();
            pfc.AddFontFile(fontPath);
            return new Font(pfc.Families[0], size, FontStyle.Regular, GraphicsUnit.Point);
        }

        return new Font(fontName ?? "Tahoma", size, FontStyle.Regular, GraphicsUnit.Point);
    }

    private static float MeasureLineHeight(Graphics g, Font font, StringFormat format)
    {
        return g.MeasureString("آ", font, int.MaxValue, format).Height;
    }

    // پارس تگ‌های <color ...>...</color>
    private static List<TextRun> ParseColoredRuns(string text, Color defaultColor)
    {
        var runs = new List<TextRun>();
        var sb = new StringBuilder();
        Color currentColor = defaultColor;

        int i = 0;
        while (i < text.Length)
        {
            if (IsColorOpenTag(text, i, out string colorToken, out int tagEnd))
            {
                FlushBufferAsRun();
                currentColor = ParseColorOrDefault(colorToken, defaultColor);
                i = tagEnd + 1;
                continue;
            }

            if (IsColorCloseTag(text, i, out int closeEnd))
            {
                FlushBufferAsRun();
                currentColor = defaultColor;
                i = closeEnd + 1;
                continue;
            }

            sb.Append(text[i]);
            i++;
        }

        FlushBufferAsRun();
        return runs;

        void FlushBufferAsRun()
        {
            if (sb.Length > 0)
            {
                runs.Add(new TextRun(sb.ToString(), currentColor));
                sb.Clear();
            }
        }
    }

    private static bool IsColorOpenTag(string text, int index, out string colorToken, out int tagEnd)
    {
        colorToken = null;
        tagEnd = -1;

        if (!StartsWithIgnoreCase(text, index, "<color"))
            return false;

        int close = text.IndexOf('>', index);
        if (close < 0) return false;

        string inside = text.Substring(index + 6, close - (index + 6)).Trim(); // بعد از "color"
        colorToken = inside;
        tagEnd = close;
        return true;
    }

    private static bool IsColorCloseTag(string text, int index, out int tagEnd)
    {
        tagEnd = -1;
        if (!StartsWithIgnoreCase(text, index, "</color>"))
            return false;

        tagEnd = index + "</color>".Length - 1;
        return true;
    }
    private static bool StartsWithIgnoreCase(string text, int index, string value)
    {
        if (text == null || value == null) return false;
        if (index < 0 || index + value.Length > text.Length) return false;

        return string.Compare(text, index, value, 0, value.Length, StringComparison.OrdinalIgnoreCase) == 0;
    }
    private static Color ParseColorOrDefault(string token, Color defaultColor)
    {
        if (string.IsNullOrWhiteSpace(token))
            return defaultColor;

        try
        {
            // پشتیبانی از نام رنگ و #RRGGBB
            return ColorTranslator.FromHtml(token.Trim());
        }
        catch
        {
            return defaultColor;
        }
    }

    // تقسیم Runها بر اساس \n و حفظ خطوط خالی
    private static List<List<TextRun>> SplitRunsByNewline(List<TextRun> runs)
    {
        var lines = new List<List<TextRun>>();
        var current = new List<TextRun>();

        foreach (var run in runs)
        {
            string[] parts = run.Text.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    current.Add(new TextRun(parts[i], run.Color));

                if (i < parts.Length - 1)
                {
                    lines.Add(current);
                    current = new List<TextRun>();
                }
            }
        }

        lines.Add(current);
        return lines;
    }

    // شکست دستی خطوط با حفظ فاصله‌ها و رنگ‌ها (کاراکتری)
    private static List<List<TextRun>> BreakLineRunsByWidth(
        Graphics g,
        List<TextRun> lineRuns,
        Font font,
        int maxWidth,
        StringFormat format)
    {
        var result = new List<List<TextRun>>();

        string currentLineText = "";
        var currentLineRuns = new List<TextRun>();

        int totalLen = lineRuns.Sum(r => r.Text?.Length ?? 0);
        if (totalLen == 0)
        {
            result.Add(new List<TextRun>()); // خط خالی
            return result;
        }

        foreach (var run in lineRuns)
        {
            if (string.IsNullOrEmpty(run.Text))
                continue;

            foreach (char ch in run.Text)
            {
                string testLine = currentLineText + ch;
                var size = g.MeasureString(testLine, font, int.MaxValue, format);

                if (size.Width <= maxWidth || currentLineText.Length == 0)
                {
                    AppendCharToRuns(currentLineRuns, ch, run.Color);
                    currentLineText = testLine;
                }
                else
                {
                    result.Add(currentLineRuns);

                    currentLineRuns = new List<TextRun>();
                    AppendCharToRuns(currentLineRuns, ch, run.Color);
                    currentLineText = ch.ToString();
                }
            }
        }

        if (currentLineRuns.Count > 0)
            result.Add(currentLineRuns);

        return result;
    }

    private static void AppendCharToRuns(List<TextRun> runs, char ch, Color color)
    {
        if (runs.Count > 0 && runs[runs.Count - 1].Color == color)
        {
            runs[runs.Count - 1].Text += ch;
        }
        else
        {
            runs.Add(new TextRun(ch.ToString(), color));
        }
    }

    private static void DrawColoredRunsOnSingleLine(
        Graphics g,
        IReadOnlyList<TextRun> runs,
        Font font,
        RectangleF rect,
        bool rtl)
    {
        var format = new StringFormat(StringFormatFlags.NoWrap
                                      | StringFormatFlags.MeasureTrailingSpaces);

        if (rtl)
            format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

        float x = rtl ? rect.Right : rect.Left;
        float y = rect.Top;

        foreach (var run in runs)
        {
            if (string.IsNullOrEmpty(run.Text))
                continue;

            var size = g.MeasureString(run.Text, font, int.MaxValue, format);

            float drawX = rtl ? x : x;

            using (var brush = new SolidBrush(run.Color))
            {
                g.DrawString(run.Text, font, brush, new PointF(drawX, y), format);
            }

            // حرکت کرسر
            x = rtl ? (x - size.Width) : (x + size.Width);
        }
    }

}
