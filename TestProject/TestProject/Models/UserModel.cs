using System.Net.Cache;
using System;
using System.Linq;
using System.Threading.Tasks;
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
      public List<Class> requiredClasses {get; set;}
      public List<UserSubject> userClasses {get; set;}
      public UserInfo userInfo {get; set;}
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
            + this.reference + "\n";
        return result;
      }
      // TODO 사용자 데이터가 필요함
      public bool GetRuleChecked()
      {
        // if(Convert.ToInt32(this.number) < 6)
        //   return;
        bool isRuleSatisfied = false;
        List<Class> reqClasses = this.requiredClasses;
        
        // 전체학점
        int totalCredit = 0;
        foreach(UserSubject userClass in this.userClasses)
        {
          totalCredit += Convert.ToInt32(userClass.credit);
        }
        UserInfo userInfo = this.userInfo;
        // todo 전산학 예외

        string userOX = "X"; // 사용자 OX
        // -------------------------------- 

        // 0: 대소비교, 1: OX, 2: 목록중선택, 3: 목록전체필수
        int flag = this.flag;
        int userCredit = 0;
        switch(flag)
        {
          case 0: // 대소비교 (학점, 평균학점 등)
            if(!this.singleInput.Contains("예시"))
            {
              if(this.question.Contains("공통교양")) 
                userCredit = userInfo.publicLibCredit;
              if(this.question.Contains("기본소양")) 
                userCredit = userInfo.basicLibCredit;
              // TODO: 수학,과학,전산학 세부구분
              if(this.question.Contains("MSC") || this.question.Contains("BSM"))
                userCredit = userInfo.mscCredit;

              // 전공과목 기준
              // TODO: 전필, 전공전문 세분화, 공과대공통과목, 개별연구 예외처리 등
              if(this.question.Contains("전공"))
              {
                userCredit = userInfo.majorCredit;
                Console.WriteLine(userCredit);
                Console.WriteLine(this.userInfo.majorCredit);
              }
              if(this.question.Contains("설계"))
              {
                // 종합설계 이수를 위한 설계과목 학점
                if(this.question.Contains("종합")) 
                  userCredit = this.userInfo.majorDesignCredit; // TODO 종합설계 과목 학점 빼야함
                else // 설계 이수학점 기준
                  userCredit = this.userInfo.majorDesignCredit;
              }

              if(userCredit >= Convert.ToInt32(this.singleInput))
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
            foreach(UserSubject userClass in this.userClasses)
            {
              foreach(Class reqClass in this.requiredClasses)
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
            foreach (UserSubject userClass in this.userClasses)
            {
              foreach (Class reqClass in this.requiredClasses)
              {
                if (userClass.classCode == reqClass.classCode)
                {
                    count += 1;
                    break;
                }
              }
            }
            if (count >= reqClasses.Count)
                isRuleSatisfied = true;
            break;
        default:
            break;
        }
        return isRuleSatisfied;
      }
    }
    public class UserSubject
    {
        public string year { get; set; }
        public string semester { get; set; }

        public string completionDiv { get; set; }
        public string completionDivField { get; set; }

        public string classCode { get; set; }
        public string className { get; set; }
        public string credit { get; set; }

        public string engineeringFactor { get; set; }
        public string engineeringFactorDetail { get; set; }
        public string english { get; set; }
    }
    public class UserInfo
    {
        public int publicLibCredit { get; set; }
        public int basicLibCredit { get; set; }
        public int majorCredit { get; set; }
        public int majorDesignCredit { get; set; }
        public int mscCredit { get; set; }
        public int englishCredit { get; set; }

        public List<string> publicClasses = new List<string>();//기초교양 수강 목록
        public List<string> basicClasses = new List<string>();//기본소양 수강 목록
        public List<string> mscClasses = new List<string>();//MSC 수강 목록
        public List<string> majorClasses = new List<string>();//전공 수강 목록
        public List<string> majorEssentialList = new List<string>();//전공필수 수강 목록
        public List<string> majorDesignList = new List<string>();//전공설계 수강 목록
        public List<string> englishList = new List<string>();//영어강의 수강 목록

        public void GetUserInfo(List<UserSubject> userSubject_)
        {
            this.publicLibCredit = 0; 
            this.basicLibCredit = 0; 
            this.majorCredit = 0; 
            this.majorDesignCredit = 0; 
            this.mscCredit = 0; 
            this.englishCredit = 0;

            foreach (UserSubject userSubject in userSubject_)
            {
                if (userSubject.engineeringFactorDetail == "기초교양(교필)")
                {
                    publicLibCredit += Convert.ToInt32(userSubject.credit);
                    this.publicClasses.Add(userSubject.classCode);
                }
                if (userSubject.engineeringFactorDetail == "기본소양")
                {
                    basicLibCredit += Convert.ToInt32(userSubject.credit);
                    this.basicClasses.Add(userSubject.classCode);
                }
                if (userSubject.engineeringFactor == "MSC/BSM")
                {
                    mscCredit += Convert.ToInt32(userSubject.credit);
                    this.mscClasses.Add(userSubject.classCode);
                }
                if (userSubject.engineeringFactor == "전공")
                {
                    majorCredit += Convert.ToInt32(userSubject.credit);
                    if (userSubject.completionDiv == "전필")
                    {
                        this.majorEssentialList.Add(userSubject.classCode);
                    }
                    if (userSubject.engineeringFactorDetail == "전공설계")
                    {
                        majorDesignCredit += Convert.ToInt32(userSubject.credit);
                        this.majorClasses.Add(userSubject.classCode);
                        continue;
                    }
                    this.majorClasses.Add(userSubject.classCode);
                }
                if (userSubject.english == "영어")
                {
                    englishCredit += Convert.ToInt32(userSubject.credit);
                    this.englishList.Add(userSubject.classCode);
                }
            }
        }
    }
}
