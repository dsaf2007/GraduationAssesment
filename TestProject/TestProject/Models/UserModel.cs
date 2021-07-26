﻿using System.Linq;
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
      // 전공 
      public int design { get; set; }
      public int essential { get; set; }

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
            + this.question + " " + this.singleInput + " "
            + this.flag + " " + this.reference + "\n";
        return result;
      }
      // TODO 사용자 데이터가 필요함
      public bool check()
      {
        bool isRuleSatisfied = false;
        List<Class> reqClasses = new List<Class>();
        
        // dummy data
        int userCredit = 83; // 사용자 학점
        string userOX = "X"; // 사용자 OX
        List<Class> userClasses = new List<Class>();
        // 공통교양 dummy data
        userClasses.Add(new Class("RGC1001", "자아와명상1", 1, 2021));
        // userClasses.Add(new Class("RGC1002", "자아와명상2", 1, 2021));
        userClasses.Add(new Class("RGC1003", "나의삶나의비전", 1, 2021));

        // 0: 대소비교, 1: OX, 2: 목록중선택, 3: 목록전체필수
        int flag = this.flag;
        // int flag = 3;
        // 수업 목록 초기화
        if(flag >= 2)
        {
            for(int i = 0 ; i < this.multiInput.Length; i++)
            {
              string[] classInfo = this.multiInput[i].Split();
              reqClasses.Add(new Class(
                classInfo[0],
                classInfo[1],
                Convert.ToInt32(classInfo[2]),
                Convert.ToInt32(classInfo[3])
              ));
            }
        }
        switch(flag)
        {
          case 0: // 대소비교 (학점, 평균학점 등)
            if(userCredit >= Convert.ToDouble(this.singleInput))
              isRuleSatisfied = true;
            break;
          case 1: // OX
            // TODO: OX가 좀 복잡함. 특정 학점의 인정/비인정, 대상/비대상 등
            if(this.singleInput.Trim() == userOX.Trim())
              isRuleSatisfied = true;
            break;
          case 2: // 최소한 하나 만족
            foreach(Class userClass in userClasses)
            {
              foreach(Class reqClass in reqClasses)
              {
                if(userClass.classCode == reqClass.classCode)
                {
                  isRuleSatisfied = true;
                  break;
                }
              }
              if(isRuleSatisfied)
                break;
            }
            break;
          case 3:
            int count = 0;
            foreach(Class userClass in userClasses)
            {
              foreach(Class reqClass in reqClasses)
              {
                if(userClass.classCode == reqClass.classCode)
                {
                  count += 1;
                  break;
                }
              }
            }
            if(count == reqClasses.Count)
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
