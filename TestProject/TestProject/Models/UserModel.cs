using System.Collections.Generic;
namespace ReadExcel.Models
{
    public class UserModel
    {
        public string type { get; set; }
        public string number { get; set; }
        public string question { get; set; }
        public string input { get; set; }
        public string flag { get; set; }
        public string reference { get; set; }

        public override string ToString()
        {
            return this.type + "_" + this.number + "_" + this.question + "_" 
              + this.input + "_" + this.flag + (this.reference == "" ? "" : "_") + this.reference + "\n";
        }
        public int getQuestionType()
        {
          if(this.question.Length <= 0)
            return -1;
          
          return 0;
        }
    }

    public class Math
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + "\n";
        }
    }
    public class BasicLiberalArts
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + "\n";
        }
    }

    public class BasicKnowledge
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + "\n";
        }
    }
    public class ScienceExperiment
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + "\n";
        }
    }
    public class MSC
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + "\n";
        }
    }
    public class MajorRequired
    {
        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }
        public string year { get; set; }
        public string project { get; set; }
        public override string ToString()
        {
            return this.classCode + " " + this.className + " " + this.credit + " " + this.year + " " + this.project + "\n";
        }
    }
    public class Class
    {
      // 학수번호
      public string classCode;
      // 과목명
      public string className; 
      // 학점
      public int credit;
      // 개설년도
      public int year;
      // (교양)
      public string category;
      // 전공 
      public int design;
      public int essential;

      public Class(string classCode, string className, int credit, int year)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.year = year;
      }
      public Class(string classCode, string className, int credit, int year, string category)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.year = year;
        this.category = category;
      }
      public Class(string classCode, string className, int credit, int year, int design, int essential)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.year = year;
        this.design = design;
        this.essential = essential;
      }
    }
    
    public class Rule
    {
      // 구분 (교양, 전공, 졸업요건, 예외)
      public int type;
      // 응답유형(대소비교, 과목 목록 선택)
      public int flag;
      // 질문
      public string question;
      // 기준에서 요구하는 학점
      public int requiredCredit;
      // 선택형(O/X)
      public char requiredOX;
      // 기준에서 요구하는 대상 과목
      public List<Class> requiredClasses;

      public override string ToString()
      {
          return this.type + this.question;
      }

      public Rule(int type, int flag, string question)
      {
        this.type = type;
        this.flag = flag;
        this.question = question;
      }
      public Rule(int type, int flag, string question, int requiredCredit)
      {
        this.type = type;
        this.flag = flag;
        this.question = question;
        this.requiredCredit = requiredCredit;
      }
      public Rule(int type, int flag, string question, char requiredOX)
      {
        this.type = type;
        this.flag = flag;
        this.question = question;
        this.requiredOX = requiredOX;
      } 
      public Rule(int type, int flag, string question, List<Class> requiredClasses)
      {
        this.type = type;
        this.flag = flag;
        this.question = question;
        this.requiredClasses = requiredClasses;
      } 
      public bool check()
      {
        // User's dummy data
        // Rule 만족 여부
        bool isSatisfied = false;
        int flag = this.flag;
        // 사용자가 수강한 과목
        // TODO: 과목의 성격(전공, 공교 등) 구분
        Class c1 = new Class("PRI2021", "미적분학1", 3, 2019);
        Class c2 = new Class("PRI2022", "확률및통계학", 3, 2020);
        List<Class> tempUserClasses = new List<Class>();
        tempUserClasses.Add(c1);
        tempUserClasses.Add(c2);
        int tempUserCredit = 0;
        for(int i = 0 ; i < tempUserClasses.Count; i++)
        {
          tempUserCredit += tempUserClasses[i].credit;
        }
        int count = 0;
        // switch(this.flag)
        switch(flag)
        {
          case 0:
            if(tempUserCredit >= this.requiredCredit)
              isSatisfied = true;
            break;
          case 1:
            // 선택 포함
            foreach(Class tempClass in requiredClasses)
            {
              // 하나라도 만족
              if(tempClass.classCode == tempUserClasses[0].classCode)
                isSatisfied = true;
            }
            break;
          case 2:
            // 모두 포함
            foreach(Class tempClass in requiredClasses)
            {
              foreach(Class tempUserClass in tempUserClasses)
              {
                if(tempClass.classCode == tempUserClass.classCode)
                {
                  count += 1;
                  break;
                }
              }
            }
            if(count == requiredClasses.Count)
              isSatisfied = true;
            break;
          default:
            break;
        }
        return isSatisfied;
      }
    }
}
