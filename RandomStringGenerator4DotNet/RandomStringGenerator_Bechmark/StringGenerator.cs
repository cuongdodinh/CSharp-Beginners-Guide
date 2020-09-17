using System;
using System.Collections.Generic;

namespace RandomStringGenerator
{
    public partial class StringGenerator
    {
        public int MinUpperCaseChars { get; set; }

        public int MinLowerCaseChars { get; set; }

        public int MinNumericChars { get; set; }

        public int MinSpecialChars { get; set; }

        public CharType FillRest { get; set; }

        public StringGenerator()
        {
        }

        public StringGenerator(int minUpperCaseChars, int minLowerCaseChars, 
            int minNumericChars, int minSpecialChars, CharType fillRest)
        {
            this.MinUpperCaseChars = minUpperCaseChars;
            this.MinLowerCaseChars = minLowerCaseChars;
            this.MinNumericChars = minNumericChars;
            this.MinSpecialChars = minSpecialChars;
            this.FillRest = fillRest;
        }

        public string GenerateString(int length)
        {
            int sum = this.MinUpperCaseChars + this.MinLowerCaseChars +
                this.MinNumericChars + this.MinSpecialChars;

            if (length < sum)
                throw new ArgumentException("length parameter must be valid!");

            List<char> chars = new List<char>();

            if (this.MinUpperCaseChars > 0)
            {
                chars.AddRange(GetUpperCaseChars(this.MinUpperCaseChars));
            }

            if (this.MinLowerCaseChars > 0)
            {
                chars.AddRange(GetLowerCaseChars(this.MinLowerCaseChars));
            }

            if (this.MinNumericChars > 0)
            {
                chars.AddRange(GetNumericChars(this.MinNumericChars));
            }

            if (this.MinSpecialChars > 0)
            {
                chars.AddRange(GetSpecialChars(this.MinSpecialChars));
            }

            int restLength = length - chars.Count;

            switch (this.FillRest)
            {
                case (CharType.UpperCase):
                    chars.AddRange(GetUpperCaseChars(restLength));
                    break;
                case (CharType.LowerCase):
                    chars.AddRange(GetLowerCaseChars(restLength));
                    break;
                case CharType.Numeric:
                    chars.AddRange(GetNumericChars(restLength));
                    break;
                case CharType.Special:
                    chars.AddRange(GetSpecialChars(restLength));
                    break;
                default:
                    chars.AddRange(GetLowerCaseChars(restLength));
                    break;
            }

            return GenerateStringFromList(chars);
        }





        private List<char> GetUpperCaseChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToUpper(Convert.ToChar(random.Next(65, 90))));
            }

            return result;
        }

        private List<char> GetLowerCaseChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToLower(Convert.ToChar(random.Next(97, 122))));
            }

            return result;
        }

        private List<char> GetNumericChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Convert.ToChar(random.Next(0, 9).ToString()));
            }

            return result;
        }

        private List<char> GetSpecialChars(int count)
        {
            List<char> result = new List<char>();

            Random random = new Random();

            for (int index = 0; index < count; index++)
            {
                result.Add(Char.ToLower(Convert.ToChar(random.Next(35, 38))));
            }

            return result;
        }

        private string GenerateStringFromList(List<char> chars)
        {
            string result = string.Empty;

            Random random = new Random();

            while (chars.Count > 0)
            {
                int randomIndex = random.Next(0, chars.Count);
                result += chars[randomIndex];
                chars.RemoveAt(randomIndex);
            }

            return result;
        }
    }


    public enum CharType
    {
        LowerCase = 0,
        UpperCase = 1,
        Numeric = 2,
        Special = 3
    }
}
    