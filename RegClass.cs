using System;

namespace UserRegister
{
    public class RegClass
    {

        private string _name { get; set; }
        private string _numberGroup { get; set; }
        private string _numberBook { get; set; }
        private string _avarScore { get; set; }
        
        public RegClass (String name, String numberGroup, String numberBook, String avarScore)
        {
            this._name = name;
            this._numberGroup = numberGroup;
            this._numberBook = numberBook;
            this._avarScore = avarScore;
        }

        public String getName()
        {
            return _name;
        }

        public void setName(String name)
        {
            this._name = name;
        }
        public String getNumberGroup()
        {
            return _numberGroup;
        }

        public void setNumberGroup(String address)
        {
            this._numberGroup = address;
        }

        public String getNumberBook()
        {
            return _numberBook;
        }

        public void setNumberBook(String phone)
        {
            this._numberBook = phone;
        }

        public String getAvarScore()
        {
            return _avarScore;
        }

        public void setAvarScore(String email)
        {
            this._avarScore = email;
        }
    }
}
