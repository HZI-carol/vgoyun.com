using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jose;
using LF.Toolkit.Common;

namespace vgoyun.common.Jwt
{
    /**
     *  JWE认证过滤器
     *  JWE字符串由以下5个部分组成：
     *  
     *  BASE64URL-ENCODE(UTF8(JWE Protected Header)) ‘.’
     *  BASE64URL-ENCODE(JWE Encrypted Key) ‘.’
     *  BASE64URL-ENCODE(JWE Initialization Vector) ‘.’
     *  BASE64URL-ENCODE(JWE Ciphertext) ‘.’
     *  BASE64URL-ENCODE(JWE Authentication Tag)
     *  
     * */
    public class JweProvider
    {
        /// <summary>
        /// 签名算法
        /// </summary>
        public const JweAlgorithm Algorithm = JweAlgorithm.A128KW;

        /// <summary>
        /// 加密方式
        /// </summary>
        public const JweEncryption Encryption = JweEncryption.A128CBC_HS256;

        public static string Encode(string key, int uid, int expires)
        {
            byte[] keys = HashAlgorithmProvider.ComputeHash("MD5", key);

            var payload = new JwePayload
            {
                iat = DateTimeUtil.Timestamp,
                exp = DateTimeUtil.GetTimestamp(DateTime.Now.AddMinutes(expires)),
                uid = uid,
            };

            return JWT.Encode(payload, keys, Algorithm, Encryption);
        }

        /// <summary>
        /// 解析jwe头部信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JweHeader Headers(string token)
        {
            return JWT.Headers<JweHeader>(token);
        }

        /// <summary>
        /// 解析Token并返回payload部分信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static JwePayload Decode(string token, string key)
        {
            byte[] keys = HashAlgorithmProvider.ComputeHash("MD5", key);

            return JWT.Decode<JwePayload>(token, keys);
        }
    }
}
