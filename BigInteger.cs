using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_SecureX
{
    internal class BigInteger
    {
        public List<byte> digits; // digits stored in little-endian order (LSB first)

        //El Code Da b3mel override l Logical operators zy : == , >= , !=, << , >> , & 
        public static bool operator <(BigInteger a, BigInteger b) => a.compare(b) < 0; //O(N)
        public static bool operator >(BigInteger a, BigInteger b) => a.compare(b) > 0;//O(N)
        public static bool operator ==(BigInteger a, BigInteger b) => a.compare(b) == 0;//O(N)
        public static bool operator !=(BigInteger a, BigInteger b) => !(a == b);//O(N)
        public static bool operator <=(BigInteger a, BigInteger b) => a.compare(b) <= 0;//O(N)
        public static bool operator >=(BigInteger a, BigInteger b) => a.compare(b) >= 0;//O(N)
        public static BigInteger operator &(BigInteger a, BigInteger b)//O(1)
        {
            if (b.digits.Count == 1 && b.digits[0] == 1)//O(1)
            {
                return new BigInteger(a.digits[0] & 1);//O(1)
            }

            throw new NotImplementedException("Bitwise AND only implemented for (BigInteger & 1)");//O(1)
        }
        public static BigInteger operator >>(BigInteger a, int shift)//O(S* N log N) //checked
        {
            BigInteger result = new BigInteger(a);//O(1)

            for (int i = 0; i < shift; i++)//O(S)
            {
                result = floor(result, new BigInteger(2));//O(N log N)+O(log N)= O(N log N)
            }

            return result;//O(1)
        }
        public static BigInteger operator <<(BigInteger a, int shift)//O(S* N^1.585) //checked
        {
            BigInteger result = new BigInteger(a);//O(1)

            for (int i = 0; i < shift; i++)//O(S)
            {
                result = Multiply(result, new BigInteger(2));//O(N^1.585)+O(log N)= O(N^1.585)
            }

            return result;//O(1)
        }

        // the constructors
        public BigInteger()//O(1)
        {
            digits = new List<byte>() { 0 };//O(1)
        }
        public BigInteger(BigInteger a)//O(1)
        {
            digits = a.digits;//O(1)
        }
        public BigInteger(string number)//T(N)=N+N then it is O(N) //checked
        {

            digits = new List<byte>();//O(1)
            foreach (char c in number.Reverse())//O(N) //reverse is O(1)
            {

                digits.Add((byte)(c - '0'));//O(1)
            }

            removeZeros();//O(N)
            if (digits.Count == 0)//O(1)
                digits.Add(0);//O(1)
        }
        public BigInteger(List<byte> digits)//O(N)
        {
            this.digits = new List<byte>(digits);//O(N)//because i'm copying the entire input list into a new list 
            removeZeros();//O(N)
            if (this.digits.Count == 0)//O(1)
                this.digits.Add(0);//O(1)
        }
        public BigInteger(int num)//O(log N)
        {
            digits = new List<byte>();//O(1)
            if (num == 0)//O(1)
            {
                digits.Add(0);//O(1)

            }

            while (num > 0)//O(log N)
            {
                digits.Add((byte)(num % 10));//O(1)
                num /= 10;//O(1)
            }

        }

        // shyl alasfar
        private void removeZeros() //O(N)
        {
            for (int i = digits.Count - 1; i > 0; i--)//O(N)
            {
                if (digits[i] == 0)//O(1)
                    digits.RemoveAt(i);//O(1)
                else
                    break;//O(1)
            }
        }

        // Convert a byte array to a BigInteger (base 256)
        public static BigInteger BytesToBigInteger(byte[] bytes)
        {
            BigInteger result = new BigInteger(0);
            BigInteger base256 = new BigInteger(256);
            for (int i = bytes.Length - 1; i >= 0; i--) // Big-endian to little-endian logic
            {
                result = Add(Multiply(result, base256), new BigInteger((int)bytes[i]));
            }
            return result;
        }

        // Convert a BigInteger back to bytes (base 256)
        public static byte[] BigIntegerToBytes(BigInteger number)
        {
            List<byte> bytes = new List<byte>();
            BigInteger base256 = new BigInteger(256);
            while (number > new BigInteger(0))
            {
                BigInteger[] divmod = Div(number,base256); // returns [quotient, remainder] Div
                number = divmod[0];
                bytes.Add((byte)divmod[1].ToInt()); // assuming you have ToInt() or similar
            }
            bytes.Reverse(); // Convert to big-endian for UTF-8 decoding
            return bytes.ToArray();
        }

        public int ToInt()
        {
            int result = 0;
            int multiplier = 1;

            foreach (byte digit in digits)
            {
                result += digit * multiplier;
                multiplier *= 10;
            }

            return result;
        }


        // 3shan a3ml override l operators zy + , - , * , /
        public static BigInteger Add(BigInteger firstNum, BigInteger secondNum)//T(N)=N+N so it is O(N) //checked
        {
            List<byte> result = new List<byte>();//O(1)
            int maxLength = Math.Max(firstNum.digits.Count, secondNum.digits.Count);//O(1)
            int temp = 0;//O(1)
            byte carry = 0;//O(1)
                           // 3shan a3ml l + operator
            for (int i = 0; i < maxLength; i++)//O(N)
            {
                byte digitA = i < firstNum.digits.Count ? firstNum.digits[i] : (byte)0;//O(1)
                byte digitB = i < secondNum.digits.Count ? secondNum.digits[i] : (byte)0;//O(1)

                byte sum = (byte)(digitA + digitB + carry);//O(1)
                result.Add((byte)(sum % 10));//O(1)
                carry = (byte)(sum / 10);//O(1)
            }
            if (carry > 0)//O(1)
                result.Add(carry);//O(1)
            return new BigInteger(result);//O(N)
        }
        public static BigInteger sub(BigInteger a, BigInteger b)//T(N)=N+N so it is O(N)
        {
            if (a < b)//O(N)
            {
                Console.WriteLine("The Second Integer Is Bigger Than The First One"); //O(1) // The Requiered Class In The Task Only Supports Positive Nums
                return new BigInteger(0);//O(log N)
            }

            return new BigInteger(sub2Lists(a.digits, b.digits));//T(N)=N+N so it is O(N)
        }
        public static BigInteger Mod(BigInteger a, BigInteger b)//O(N log N)
        {
            BigInteger[] result = Div(a, b);//O(N log N)
            return result[1];//O(1)
        }
        public static BigInteger[] Div(BigInteger a, BigInteger b)//O(N log N), Total time = time per recursive call * number of recursive calls
        {
            // Base case: if a is less than b, quotient is 0 and remainder is a
            if (a < b)//O(N)
                return new BigInteger[] { new BigInteger(0), a };//O(1)

            // Calculate 2*b
            BigInteger twoB = Add(b, b);//O(N)

            // Recursive call with a and 2*b
            BigInteger[] temp = Div(a, twoB);//T(N)=T(N/2)+O(N) =  Θ(N)
            BigInteger q = temp[0];//O(1)
            BigInteger r = temp[1];//O(1)

            // Double the quotient (q = 2*q)
            q = Add(q, q);//O(N)

            // Check and adjust remainder and quotient if needed
            if (r < b)//O(N)
                return new BigInteger[] { q, r };//O(1)
            else
                return new BigInteger[] { Add(q, new BigInteger(1)), sub(r, b) };//O(N)
        }
        public static BigInteger floor(BigInteger a, BigInteger b) //O(N log N)
        {
            BigInteger[] result = Div(a, b);  //O(N log N)
            return result[0];
        }
        public static BigInteger Multiply(BigInteger a, BigInteger b)//O(N^1.585) //checked
        {
            // Check for zero multiplication
            if ((a.digits.Count == 1 && a.digits[0] == 0) || (b.digits.Count == 1 && b.digits[0] == 0))//O(1)
            {
                return new BigInteger("0"); //O(B) where B is the big int constructor complexity
            }

            // Simple multiplication  
            if (a.digits.Count <= 4 || b.digits.Count <= 4)//O(1)
            {
                List<byte> result = new List<byte>();//O(1)
                for (int i = 0; i < a.digits.Count + b.digits.Count; i++)//O(N+M) for loop on a and b digits count
                {
                    result.Add(0);//O(1)
                }

                for (int i = 0; i < a.digits.Count; i++)//O(N)
                {
                    byte carry = 0;//O(1)
                    for (int j = 0; j < b.digits.Count || carry > 0; j++)//O(M)
                    {
                        byte digitB = j < b.digits.Count ? b.digits[j] : (byte)0;//O(1)
                        int current = result[i + j] + a.digits[i] * digitB + carry;//O(1)
                        result[i + j] = (byte)(current % 10);//O(1)
                        carry = (byte)(current / 10);//O(1)
                    }
                }//total of O(N*M)

                return new BigInteger(result);//O(N)
            }

            int n = Math.Max(a.digits.Count, b.digits.Count);//O(1)
            int m = n / 2;//O(1)

            // Split a into high and low parts
            List<byte> aLDigits = new List<byte>();//O(1)
            List<byte> aHDigits = new List<byte>();//O(1)

            for (int i = 0; i < a.digits.Count; i++)//O(N)
            {
                if (i < m)//O(1)
                {
                    aLDigits.Add(a.digits[i]);//O(1)
                }
                else
                {
                    aHDigits.Add(a.digits[i]);//O(1)
                }
            }//total O(N)

            if (aLDigits.Count == 0) aLDigits.Add(0);//O(1)
            if (aHDigits.Count == 0) aHDigits.Add(0);//O(1)

            BigInteger aL = new BigInteger(aLDigits);//O(N/2)
            BigInteger aH = new BigInteger(aHDigits);//O(N/2) because i divide the a by half

            // Split b into high and low parts
            List<byte> bLDigits = new List<byte>();//O(1)
            List<byte> bHDigits = new List<byte>();//O(1)

            for (int i = 0; i < b.digits.Count; i++)//O(M)
            {
                if (i < m)//O(1)
                {
                    bLDigits.Add(b.digits[i]);//O(1)
                }
                else
                {
                    bHDigits.Add(b.digits[i]);//O(1)
                }
            }//total O(M)

            if (bLDigits.Count == 0) bLDigits.Add(0);//O(1)
            if (bHDigits.Count == 0) bHDigits.Add(0);//O(1)

            BigInteger bL = new BigInteger(bLDigits);//O(M/2)
            BigInteger bH = new BigInteger(bHDigits);//O(M/2) because i divide the b by half

            // Karatsuba
            // T(N) = 3T(N / 2) + O(N) = O(N^log2(3)) = O(N^1.585)
            BigInteger z0 = Multiply(aL, bL);//O((N/2)^1.585)
            BigInteger z2 = Multiply(aH, bH);//O((N/2)^1.585)

            // z1 = (aLow + aHigh) * (bLow + bHigh) - z0 - z2
            BigInteger aS = Add(aL, aH);//O(N)
            BigInteger bS = Add(bL, bH);//O(N)
            BigInteger z1 = sub(sub(Multiply(aS, bS), z0), z2);//O(N^1.585)+O(N)+O(N) = O(N^1.585)

            // result = z2 * 10^(2*m) + z1 * 10^m + z0
            BigInteger res1 = new BigInteger(ShiftLeft(z2.digits, 2 * m));//O(N)
            BigInteger res2 = new BigInteger(ShiftLeft(z1.digits, m));//O(N)

            return Add(Add(res1, res2), z0);//O(N)
        }
        private static List<byte> ShiftLeft(List<byte> digits, int shift) //O(N) T(N)=S+M //checked
        {
            if (digits.Count == 1 && digits[0] == 0)//O(1)
            {
                return new List<byte> { 0 };//O(1)
            }

            List<byte> result = new List<byte>();//O(1)

            for (int i = 0; i < shift; i++)//O(S)
            {
                result.Add(0);//O(1)
            }

            result.AddRange(digits);//O(M) where M equals number of elements in the collection

            return result;//O(1)
        }
        private static List<byte> sub2Lists(List<byte> a, List<byte> b)//O(N) //checked
        {
            List<byte> finalRes = new List<byte>();//O(1)
            int maxLength = Math.Max(a.Count, b.Count);//O(1)
            byte borrow = 0;//O(1)

            for (int i = 0; i < maxLength; i++)//O(N)
            {
                byte digitA = i < a.Count ? a[i] : (byte)0;//O(1)
                byte digitB = i < b.Count ? b[i] : (byte)0;//O(1)

                int diff = digitA - digitB - borrow;//O(1)
                if (diff < 0)//O(1)
                {
                    diff += 10;//O(1)
                    borrow = 1;//O(1)
                }
                else
                {
                    borrow = 0;//O(1)
                }

                finalRes.Add((byte)diff);//O(1)
            }

            return finalRes;//O(1)
        }
        // alm8arna
        public int compare(BigInteger num)//O(N) total complexity
        { // 3amelha 3shan A3ml override ll Logical Operators
            if (this.digits.Count > num.digits.Count) return 1;//O(1)
            if (this.digits.Count < num.digits.Count) return -1;//O(1)

            for (int i = this.digits.Count - 1; i >= 0; i--) //O(N) N=digits.count
            {
                if (this.digits[i] > num.digits[i]) return 1;//O(1)
                if (this.digits[i] < num.digits[i]) return -1;//O(1)
            }
            return 0;//O(1)
        }
        public static string Check(BigInteger a) //O(N log N)
        {
            BigInteger c = new BigInteger("2");
            BigInteger x = Mod(a, c); //O(N log N)
            return (x == new BigInteger(0)) ? "Even" : "Odd"; // O(N) + O(log N)
        }
        public override string ToString()
        {
            string number = "";
            for (int i = digits.Count - 1; i >= 0; i--)
            {
                number += (char)(digits[i] + '0');
            }
            return number;
        }
        public static BigInteger Sqrt(BigInteger N)//O(log(N) * N^1.585) //checked
        {
            if (N.digits.Count == 1 && N.digits[0] == 0)//O(1)
                return new BigInteger("0");//O(1)

            if (N.digits.Count == 1 && N.digits[0] == 1)//O(1)
                return new BigInteger("1");//O(1)

            BigInteger l = new BigInteger("1");//O(1)
            BigInteger h = new BigInteger(N.digits); //O(N)// Copy constructor
            BigInteger finalRes = new BigInteger("0");//O(1)

            while (l <= h)//O(log N)
            {
                BigInteger mid = floor(Add(l, h), new BigInteger("2"));//O(N log N)+ O(N)+ O(N)= O(N log N)
                BigInteger square = Multiply(mid, mid);//O(N^1.585)

                if (square == N)//O(1)
                    return mid;//O(1)

                if (square < N)//O(1)
                {
                    l = Add(mid, new BigInteger("1"));//O(N)+O(1)= O(N)
                    finalRes = mid;//O(1) // Store floor value
                }
                else
                {
                    h = sub(mid, new BigInteger("1"));//O(N)+O(1)= O(N)
                }
            }//total complexity is O(log(N)*N ^ 1.585)

            return finalRes;//O(1)
        }
    }
}