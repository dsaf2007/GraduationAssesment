using System.Linq;
using System;
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
      public string classCode { get; set; }
      // 과목명
      public string className { get; set; }
      // 학점
      public int credit { get; set; }
      // 개설년도
      public int year { get; set; }
      // (교양)
      public string category { get; set; }
      // 설계학점
      public int design { get; set; }
      // public int essential { get; set; }
      public Class() {}
      public Class(string classCode, string className, int credit, int year)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.year = year;
      }
      public Class(string classCode, string className, int credit, string category, int year)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.category = category;
        this.year = year;
      }
      public Class(string classCode, string className, int credit, int design, int year)
      {
        this.classCode = classCode;
        this.className = className;
        this.credit = credit;
        this.design = design;
        this.year = year;
        // this.essential = essential;
      }
      public override string ToString()
      {
       string result = String.Format("{0} {1} {2} ", this.classCode, this.className, this.credit);
       if(this.design != -1)
       {
         result += this.design.ToString() + " ";
       }
       
       return result + this.year.ToString() + "\n";
      }
    }
    
    public class Rule
    {
      // 구분 (교양, 전공, 졸업요건, 예외)
      public string type { get; set; }
      // 일련번호
      public string number { get; set; }
      // 질문
      public string question { get; set; }
      // 엑셀 입력 데이터
      public string singleInput { get; set; }
      public string[] multiInput { get; set; }
      // 응답유형
        /* 
        0: 대소비교
        1: OX
        2: 목록중선택
        3: 목록전체필수
        */
      public int flag { get; set; }
      // 비고
      public string reference { get; set; }

      public override string ToString()
      {
        string result = this.type + " "
            + this.number + " " 
            + this.question + " " 
            + this.singleInput + " "
            + this.flag + " " 
            + this.reference + "\n";
        return result;
      }
      // TODO 사용자 데이터가 필요함
      public bool check()
      {
        // if(Convert.ToInt32(this.number) < 6)
        //   return;
        bool isRuleSatisfied = false;
        List<Class> reqClasses = new List<Class>();
        
        // User dummy data ----------------
        int userCredit = 80; // 사용자 학점
        string userOX = "X"; // 사용자 OX
        List<Class> userClasses = new List<Class>();
        // 공통교양 dummy data
        userClasses.Add(new Class("RGC1001", "자아와명상1", 1, 2021));
        userClasses.Add(new Class("RGC1002", "자아와명상2", 1, 2021));
        userClasses.Add(new Class("RGC1003", "나의삶나의비전", 1, 2021));
        userClasses.Add(new Class("CSE2016", "창의적공학설계", 3, 3, 2016));
        // -------------------------------- 

        // 0: 대소비교, 1: OX, 2: 목록중선택, 3: 목록전체필수
        int flag = this.flag;
        // int flag = 3;
        // 수업 목록 초기화
        if(flag >= 2)
        {
          // 해당 룰을 만족하기 위해 필요한 과목이 없으므로
          if(this.multiInput == null || this.multiInput.Length <= 0)
            return true;

          // sheet가 비었을 경우
          if(multiInput[0].Split().Length <= 1) 
            return true;

          for(int i = 0 ; i < this.multiInput.Length; i++)
          {
            string[] classInfo = this.multiInput[i].Split();
            for(int j = 0; j < classInfo.Length; j++)
            {
              // 엑셀 row에 빈칸이 있을 경우
              if(String.IsNullOrEmpty(classInfo[j]))
              {
                // 학수번호나 과목명(j= 0, 1)이 비었다 => 에러
                // 그외 숫자칸이 비었다 => 0 대입
                // (위에서 걸렀음) 대상 목록이 아예 없는 경우 (정상)
                  if(j == 0 || j == 1)
                    return false;
                  else
                    classInfo[j] = "0";
              }
            }
            Class aClass = new Class{
                classCode=classInfo[0],
                className=classInfo[1],
                credit=Convert.ToInt32(classInfo[2]),
                year=Convert.ToInt32(classInfo[3])
            };
            if(classInfo.Length > 5)
            {
              aClass.design = Convert.ToInt32(classInfo[3]);
              aClass.year = Convert.ToInt32(classInfo[4]);
            }
            reqClasses.Add(aClass);
          }
        }
        switch(flag)
        {
          case 0: // 대소비교 (학점, 평균학점 등)
            if(!this.singleInput.Contains("예시"))
            {
              if(userCredit >= Convert.ToDouble(this.singleInput))
                isRuleSatisfied = true;
            }
            break;
          case 1: // OX
            // OX가 좀 복잡함. 특정 학점의 인정/비인정, 대상/비대상 등에 따라
            // 다른 룰에 영향 미침 (예) 졸업논문 대체가능 ox ?
            if(!this.singleInput.Contains("예시"))
            {
              if(!("OXox".Contains(this.singleInput) && "OXox".Contains(userOX)))
                return false;
              if(this.singleInput.Trim().ToUpper() == userOX.Trim().ToUpper())
                isRuleSatisfied = true;
            }
            break;
          case 2: // 최소한 하나 만족
            foreach(Class userClass in userClasses)
            {
              foreach(Class reqClass in reqClasses)
              {
                if(userClass.classCode == reqClass.classCode)
                {
                  return true;
                }
              }
            }
            break;
        case 3: // 전체 만족
            int count = 0;
            foreach (Class userClass in userClasses)
            {
              foreach (Class reqClass in reqClasses)
              {
                if (userClass.classCode == reqClass.classCode)
                {
                    count += 1;
                    break;
                }
              }
            }
            if (count == reqClasses.Count)
                isRuleSatisfied = true;
            break;
        default:
            break;
        }
        return isRuleSatisfied;
      }

    //   public bool check()
    //   {
    //     // User's dummy data
    //     // Rule 만족 여부
    //     bool isSatisfied = false;
    //     int flag = this.flag;
    //     // 사용자가 수강한 과목
    //     // TODO: 과목의 성격(전공, 공교 등) 구분
    //     Class c1 = new Class("PRI2021", "미적분학1", 3, 2019);
    //     Class c2 = new Class("PRI2022", "확률및통계학", 3, 2020);
    //     List<Class> tempUserClasses = new List<Class>();
    //     tempUserClasses.Add(c1);
    //     tempUserClasses.Add(c2);
    //     int tempUserCredit = 0;
    //     for(int i = 0 ; i < tempUserClasses.Count; i++)
    //     {
    //       tempUserCredit += tempUserClasses[i].credit;
    //     }
    //     int count = 0;
    //     // switch(this.flag)
    //     switch(flag)
    //     {
    //       case 0:
    //         if(tempUserCredit >= this.requiredCredit)
    //           isSatisfied = true;
    //         break;
    //       case 1:
    //         // 선택 포함
    //         foreach(Class tempClass in requiredClasses)
    //         {
    //           // 하나라도 만족
    //           if(tempClass.classCode == tempUserClasses[0].classCode)
    //             isSatisfied = true;
    //         }
    //         break;
    //       case 2:
    //         // 모두 포함
    //         foreach(Class tempClass in requiredClasses)
    //         {
    //           foreach(Class tempUserClass in tempUserClasses)
    //           {
    //             if(tempClass.classCode == tempUserClass.classCode)
    //             {
    //               count += 1;
    //               break;
    //             }
    //           }
    //         }
    //         if(count == requiredClasses.Count)
    //           isSatisfied = true;
    //         break;
    //       default:
    //         break;
    //     }
    //     return isSatisfied;
    //   }
    }
}
