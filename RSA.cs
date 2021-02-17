using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RCoder
{
    class RSA
    {
        private static Encoding Encoding_UTF8 = Encoding.UTF8;

        private AsymmetricKeyParameter GetPublicKeyParameter(string keyBase64)
        {
            keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] publicInfoByte = Convert.FromBase64String(keyBase64); 
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);
            return pubKey;
        }

        private AsymmetricKeyParameter GetPrivateKeyParameter(string keyBase64)
        {
            keyBase64 = keyBase64.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] privateInfoByte = Convert.FromBase64String(keyBase64);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);
            return priKey;
        }




        public string[] GetKeys()
        {
            string[] RSAKey = new String[2];
            RSAKEY key=GetKey();
            RSAKey[0] = "-----BEGIN PRIVATE KEY-----\r\n" + key.PrivateKey + "\r\n-----END PRIVATE KEY-----";
            RSAKey[1] = "-----BEGIN PUBLIC KEY-----\r\n" + key.PublicKey + "\r\n-----END PUBLIC KEY-----";
            return RSAKey;
        }

        public struct RSAKEY
        {
            public string PublicKey
            {
                get;
                set;
            }
            public string PrivateKey
            {
                get;
                set;
            }
        }

        public RSAKEY GetKey()
        {
            //RSA密钥对的构造器  
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数  
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                new Org.BouncyCastle.Security.SecureRandom(),
                1024,   //密钥长度  
                25);
            //用参数初始化密钥构造器  
            keyGenerator.Init(param);
            //产生密钥对  
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥  
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);


            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();

            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            RSAKEY item = new RSAKEY()
            {
                PublicKey = Convert.ToBase64String(publicInfoByte),
                PrivateKey = Convert.ToBase64String(privateInfoByte)
            };

           return item;
        }



        public string Encrypt(string data, string publicKey)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密  

                engine.Init(true, GetPublicKeyParameter(publicKey.Replace("-----BEGIN PUBLIC KEY-----\r\n", "").Replace("\r\n-----END PUBLIC KEY-----", "")));
                byte[] byteData = Encoding_UTF8.GetBytes(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Convert.ToBase64String(ResultData);


        }


        public string Decrypt(String data, string privateKey)
        {
            data = data.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //解密  
            try
            {
                engine.Init(false, GetPrivateKeyParameter(privateKey.Replace("-----BEGIN PRIVATE KEY-----\r\n", "").Replace("\r\n-----END PRIVATE KEY-----", "")));
                byte[] byteData = Convert.FromBase64String(data);
                var ResultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return Encoding_UTF8.GetString(ResultData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
