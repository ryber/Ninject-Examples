using System;

namespace NinjectExamples
{
    public class Documentation : Attribute
    {
        public Documentation(string link){}
    }

    public class Warning : Attribute
    {
        public Documentation(string link) { }
    }
}