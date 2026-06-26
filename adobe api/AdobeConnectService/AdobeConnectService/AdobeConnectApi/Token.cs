using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdobeConnectService.Controllers
{
    public class GenericRequired : RequiredAttribute
    {
        public GenericRequired()
        {
            this.ErrorMessage = "وارد کردن {0} الزامی است";
        }
    }
    public class TokenModel
    {
        [Display(Name = "ایمیل"), GenericRequired, JsonRequired, DataType(DataType.EmailAddress, ErrorMessage = "ایمیل صحیح نمی باشد")]
        public string Email { get; set; }
        [Display(Name = "ایمیل"), GenericRequired, JsonRequired]
        public string Pass { get; set; }
    }
    public static class Token
    {
        private const string secret = "AdobeConnectServiceApiTokenController";
        public static string GenerateToken(TokenModel model)
        {
//            var payload = new Dictionary<string, object>
//{
//    { "Email",model.Email },
//    { "Pass", model.Pass },
//};
            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            //var token = encoder.Encode(payload, secret);
            var token = new JwtBuilder()
     .WithAlgorithm(new HMACSHA256Algorithm())
     .WithSecret(secret)
     .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
     .AddClaim("Email", model.Email)
     .AddClaim("Pass", model.Pass)
     .Build();
            return token;
        }
        public static TokenModel GetUserByToken(string token)
        {
            if (string.IsNullOrEmpty(token?.Trim()))
            {
                throw new Exception("توکن معتبر نیست");
            }
            try
            {
                //    IJsonSerializer serializer = new JsonNetSerializer();
                //    IDateTimeProvider provider = new UtcDateTimeProvider();
                //    IJwtValidator validator = new JwtValidator(serializer, provider);
                //    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                //    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                //    var json = decoder.Decode(token, secret, verify: true);
                var json = new JwtBuilder()
       .WithSecret(secret)
       .MustVerifySignature()
       .Decode(token);
                return JsonConvert.DeserializeObject<TokenModel>(json);
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Token has invalid signature");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
