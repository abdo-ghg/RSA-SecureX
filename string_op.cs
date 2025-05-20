using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class string_op
    {
        public static BigInteger StringToBigInteger(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            BigInteger result = new BigInteger(0);
            BigInteger base256 = new BigInteger(256);

            for (int i = 0; i < bytes.Length; i++)
            {
                result = BigInteger.Add(BigInteger.Multiply(result, base256), new BigInteger((int)bytes[i]));
            }

            return result;
        }

        public static string BigIntegerToString(BigInteger number)
        {
            List<byte> bytes = new List<byte>();
            BigInteger base256 = new BigInteger(256);

            while (number > new BigInteger(0))
            {
                BigInteger[] divmod = BigInteger.Div(number, base256);
                number = divmod[0];
                bytes.Add((byte)divmod[1].ToInt());
            }
            bytes.Reverse();

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}
