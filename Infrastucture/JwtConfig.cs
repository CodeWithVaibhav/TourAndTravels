using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace TourAndTravels.Infrastucture
{
    public static class JwtConfig
    {
        public static IConfiguration Configuration { get; set; }
        //To Do - this need to find some better location to store.
        private static readonly string RsaKey = @"<RSAKeyValue><Modulus>t4W9RsvpS+GHRZgrLV1qyLeOKMSzNfnkl594bVsAfaY+EBsIi58z1SU77N93S9xdmibJyDmQ6sOgIgfwebSEIPQ2ppumy574rgDNeotWI4IRHIblJy8TAVSqjOse6NYrTVRygR7XlHY3fFEMQq2ByNidm66qecSGX11siOrOZtHUn1aTu6/W+PEfJUL3K4GYy0nRczFA5tJ/vVolooUynhVPk1RgCn1tlTbSEomBLnncHs2H7+mYCUaCAk55lxzpDme5FOJ6vjupMGhRQ2rDxca2hW6M2fp0WrCxho+nw++0kHZfPa9geMqc4wiiLdGHMEx6yX9U08TDlzw30saWoQ==</Modulus><Exponent>AQAB</Exponent><P>1IxQE3de6UnMAj+DBZi6dzkXlThOTYY9PHafCMwDMOTuiwm5qe0mVcsTi4e/5HQm+FZyFHimUlZK/tnEkQgVPXg3YtbsKnR0SH/zNHLqVlJP59TCjaMi3hkL7gbxkwBSxggYB5mL2aR8XLs/xxxImc4sQhosb34dIGcfD0/QMxM=</P><Q>3Qpev+rgsGW9Y6579IuGjLtfu/AyoDK2Ap3xNslq2hXA+567zkIN+c3ieKSsPTXcEVaErHSVoa+l9+1r2ZBvxOfZ5UqRspdqBI4ChMr33pBP4qe12rVuxJPYcHIiPf5g/OfnY8/68YXcE/6bIAes9Bio6HUPukb/PlWjqzSG0fs=</Q><DP>QAA4JIRvSeP00EB7nXXNwBSq6z3XJjTjv48geC+mTlTBF5DIHUjRoEfUDNHFkG8BRQoSs+NYgbaGQpMkQ1+hta67TsHUvzvtv973RrXLpk6GzRIduzSCTUpRc00X9OHGCudpBPDRUanb4LhpjfUQT2/rl8P12WSqNJWYnq2zzY0=</DP><DQ>otqKFoIZiz1aPQAICZNmzXcbwJ7m3cqe+OyfFItvc0BFz90SZ5OZMmwzKEnDMNHm0stYsqqut9JuyGyfYksdXgioLFw1XkxaawKp7maQGuVeRhLkVEAKXUDEXYxpmB08HflKLOrF636BOCYE6qNdFPa6M5JTxR2b3rRAAtqPXlM=</DQ><InverseQ>IquEwdt0+lROb5BhEEcpcqhRfSNb2IOxgP4FrcfUjTZ98cVMCni1XUcX3FQkEqmEO/jv8d3/lARCiZrrt6n+PubeRSCRwRdZDZ5zV40XbPgA731rypPWG4LdCJ7ACsP6Tq15G3wtVtnofAIqAZ4m+0SL9aBsS8ZQEROwkL6OGdA=</InverseQ><D>kQbhEqdlj4+D5J1h0ZG3JzLW5qS6snbuDFv7QP/fHWxYM0YJtLx0q4WnG4NktNBKL91jLBVBziQV0Y7QnHzoxVn4LiVm5BaWknsT5jliMuFAQHjhcdujezO7K21KmdlVkuCfNCZ5WlspcdZih/axT1TPCA45rcx+G7KqaHCV922Xoele+c7Wv8F1/pR9ylNgy3rB40QAF4oLgNc8MzAPJPL0mZbZRtM7HPLhQ24G45k6qLd/wUATEodxZaBVXSARLcglrlHexOs+OgKw2/6UXDGP6W0KYDHDMUL8hvVMPcUnJxWZjhkcFEfpWglHe30YPNRK1B/sUMMGIBhNy8QjGQ==</D></RSAKeyValue>";

        public static JwtIssuerOptions GetJwtIssuerOptions()
        {
            return new JwtIssuerOptions
            {
                Issuer = Configuration["JwtToken:Issuer"],
                Audience = Configuration["JwtToken:Audience"],
                ValidFor = TimeSpan.FromMinutes(Convert.ToInt32(Configuration["JwtToken:ValidFor"])),
                SigningCredentials = SigningCredentials()
            };
        }

        private static string CreatePrivateAndPublicKey()
        {
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
            RSAParameters publicKey = myRSA.ExportParameters(true);
            string publicAndPrivateKey = myRSA.ToXmlString(true);
            return publicAndPrivateKey;
        }

        public static SigningCredentials SigningCredentials() =>
            new SigningCredentials(GetSigningKey(), SecurityAlgorithms.RsaSha512);

        public static RsaSecurityKey GetSigningKey() =>
            new RsaSecurityKey(GetRSACryptoProvider());


        private static RSACryptoServiceProvider GetRSACryptoProvider()
        {
            RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
            publicAndPrivate.FromXmlString(RsaKey);
            return publicAndPrivate;
        }
    }

    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration => IssuedAt.Add(ValidFor);
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);
        public SigningCredentials SigningCredentials { get; set; }

        public Func<Task<string>> JtiGenerator =>
         () => Task.FromResult(Guid.NewGuid().ToString());
    }
}
