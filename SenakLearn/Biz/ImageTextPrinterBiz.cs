using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

public static class ImageTextPrinterBiz
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
        Color? color = null)
    {
        color = Color.Black;

        using (var ms = new MemoryStream(imageBytes))
        using (var img = Image.FromStream(ms))
        using (var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            g.DrawImage(img, 0, 0, img.Width, img.Height);

            Font font = BuildFont(fontPath, fontName, initialFontSize);

            // شکست متن (با پشتیبانی از \n)
            var lines = BreakTextManually(g, text, font, maxWidth, maxHeight, lineSpacingPx);

            var format = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Far,
                FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap
            };

            using (var brush = new SolidBrush(color.Value))
            {
                float lineHeight = MeasureLineHeight(g, font, format);
                float yCursor = y;

                foreach (var line in lines)
                {
                    var rect = new RectangleF(x, yCursor, maxWidth, lineHeight);
                    g.DrawString(line, font, brush, rect, format);
                    yCursor += lineHeight + lineSpacingPx;

                    if (yCursor > y + maxHeight) break;
                }
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

    /// <summary>
    /// شکستن دستی متن با پشتیبانی از \n و بدون Wrap خودکار
    /// </summary>
    private static List<string> BreakTextManually(
        Graphics g,
        string text,
        Font font,
        int maxWidth,
        int maxHeight,
        float lineSpacingPx)
    {
        var format = new StringFormat(StringFormatFlags.DirectionRightToLeft
                                      | StringFormatFlags.NoWrap
                                      | StringFormatFlags.MeasureTrailingSpaces);

        // خطوط خام بر اساس \n (پشتیبانی از \r\n)
        var rawLines = text.Replace("\r\n", "\n").Split('\n');

        var finalLines = new List<string>();

        foreach (var raw in rawLines)
        {
            if (raw == null)
            {
                finalLines.Add(string.Empty);
                continue;
            }

            string currentLine = "";

            // شکست دستی کاراکتری برای حفظ فاصله‌ها
            foreach (char ch in raw)
            {
                string testLine = currentLine + ch;
                var size = g.MeasureString(testLine, font, int.MaxValue, format);

                if (size.Width <= maxWidth)
                    currentLine = testLine;
                else
                {
                    if (currentLine.Length > 0)
                        finalLines.Add(currentLine);

                    currentLine = ch.ToString();
                }
            }

            // اگر خط خالی بود (مثلاً خط خالی بین دو \n)
            if (raw.Length == 0)
                finalLines.Add(string.Empty);
            else if (currentLine.Length > 0)
                finalLines.Add(currentLine);
        }

        // محدودیت ارتفاع
        float lineHeight = MeasureLineHeight(g, font, format);
        float lineAdvance = lineHeight + lineSpacingPx;
        int maxLines = (int)Math.Floor(maxHeight / lineAdvance);

        if (finalLines.Count > maxLines)
            finalLines = finalLines.GetRange(0, Math.Max(0, maxLines));

        return finalLines;
    }

}
