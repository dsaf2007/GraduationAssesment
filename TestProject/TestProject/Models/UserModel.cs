namespace ReadExcel.Models
{
    public class UserModel
    {
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
    }

    public class Math
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
    }
    public class BasicLiberalArts
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
    }

    public class BasicKnowledge
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
    }
    public class ScienceExperiment
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
    }
    public class MSC
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
    }

    public class MajorRequired
    {
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public string project { get; set; }
    }

    public class UserSubject
    {
        public string year { get; set; }
        public string semester { get; set; }
        public string completion_div { get; set; }
        public string completion_div_feild { get; set; }
        public string class_num { get; set; }
        public string class_name { get; set; }
        public string credit { get; set; }
        public string engineering_factor { get; set; }
        public string engineering_factor_detail { get; set; }
        public string english { get; set; }

    }
    public class UserCredit
    {
        public int public_lib { get; set; }
        public int basic_lib { get; set; }
        public int major { get; set; }
        public int major_arc { get; set; }
        public int msc { get; set; }
        public int english { get; set; }
    }
}
