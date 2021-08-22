using System.Runtime.CompilerServices;
using System.Net.Cache;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ExcelDataReader;

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
            if (this.question.Length <= 0)
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
        public Class() { }
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
            if (this.design != -1)
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
        // rule passed
        public bool isPassed {get; set;}
        public List<Class> requiredClasses { get; set; }
        public List<UserSubject> userClasses { get; set; }
        public UserInfo userInfo { get; set; }
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
        // check 함수는 RuleChecker로 이동!!
    }
    // 전체 rule & check list
    public class RuleManager
    {
      public List<Rule> rules {get;set;}
      public List<bool> ruleCheckedList {get;set;}
      // todo: 밑에 두개 미구현; 개별Rule에서 여전히 전체 user정보 저장하는중
      // Manager에서 관리하고 user에서는 가져다쓰도록 해야하지않나
      public UserInfo userInfo {get;set;}
      public List<UserSubject> userSubjects {get;set;}

      public RuleManager()
      {
        this.rules = new List<Rule>();
        this.ruleCheckedList = new List<bool>();
      }
      public RuleManager(List<Rule> rules)
      {
        this.rules = rules;
        this.ruleCheckedList = new List<bool>();
      }
      public void CheckAllRules()
      {
        if(this.rules.Count == 0)
          return;
        
        List<Rule> rules = this.rules;
        for(int i = 0 ; i < this.rules.Count; i++)
        {
          RuleChecker ruleChecker = new RuleChecker(rules[i]);
          ruleChecker.CheckRule();
        }
      }
    }
    // 개별 Rule Checker (클래스 분리)
    // todo: UserInfo, UserClass 등 사용자 정보(+디비?)를
    // rule이 아닌 ruleChecker 또는 Manager가 가지고 있도록 하기
    public class RuleChecker
    {
      public Rule rule {get;set;}
      public RuleChecker(Rule rule)
      {
        this.rule = rule;
      }

      public bool GetRuleChecked()
      {     
        bool isRuleSatisfied = false;
        Rule rule = this.rule;
        List<Class> reqClasses = rule.requiredClasses;

        UserInfo userInfo = rule.userInfo;
        int totalCredit = userInfo.totalCredit;

        // 0: 대소비교, 1: OX, 2: 목록중선택, 3: 목록전체필수
        int flag = rule.flag;
        double userCredit = 0;
        // 띄어쓰기 제거
        string question = Regex.Replace(rule.question, @"\s", "");
        switch (flag)
        {
            case 0: // 대소비교 (학점, 평균학점 등)
                if (!rule.singleInput.Contains("예시"))
                {
                    // 공통교양, 기본소양
                    if (question.Contains("공통교양"))
                        userCredit = userInfo.publicLibCredit;
                    if (question.Contains("기본소양"))
                        userCredit = userInfo.basicLibCredit;
                    // MSC
                    if (question.Contains("MSC") || question.Contains("BSM"))
                    {
                        userCredit = userInfo.mscCredit;
                    }
                    if (question.Contains("과학") && question.Contains("실험"))
                        userCredit = userInfo.mscScienceExperimentCredit;
                    if (question.Contains("수학이수")) // 그냥 '수학' -> 이'수학'점 에 걸림
                        userCredit = userInfo.mscMathCredit;
                    if (question.Contains("전산학"))
                        userCredit = userInfo.mscComputerCredit;
                    // 전공과목 기준
                    // TODO: 공과대공통과목, 개별연구 예외처리 등
                    if (question.Contains("전공"))
                    {
                      if(question.Contains("전문"))
                      {
                        userCredit = userInfo.majorSpecialCredit;
                      }
                      if(question.Contains("필수"))
                      {
                        userCredit = userInfo.majorEssentialCredit;
                      }
                      userCredit = userInfo.majorCredit;
                    }

                    if (question.Contains("설계"))
                    {
                        userCredit = userInfo.majorDesignCredit;
                    }
                    if(question.Contains("총취득학점"))
                      userCredit = userInfo.totalCredit;
                    if(question.Contains("평점평균"))
                      userCredit = userInfo.gradeAverage;
                    if(question.Contains("영어"))
                    {
                      if(question.Contains("전공과목수"))
                        userCredit = userInfo.englishMajorList.Count;
                      else if(question.Contains("총과목수"))
                        userCredit = userInfo.englishList.Count;
                    }
                    // Todo: 평점평균, OX 등
                    if (userCredit >= Convert.ToDouble(rule.singleInput))
                    {
                        isRuleSatisfied = true;
                    }
                }
                break;
            case 1: // OX
                    // OX가 좀 복잡함. 특정 학점의 인정/비인정, 대상/비대상 등에 따라
                    // 다른 룰에 영향 미침 (예) 졸업논문 대체가능 ox ?
                if(question.Contains("패스제")) // 외국어패스제 대상
                {
                  if(userInfo.englishPass[0] == "대상" && userInfo.englishPass[1].ToUpper() == "PASS")
                    return true;
                  else
                    return false;
                }
                if(question.Contains("영어강의")) // 영어강의 대상
                {
                  if(userInfo.englishClassPass[0] == "대상" && userInfo.englishClassPass[1].ToUpper() == "PASS")
                    return true;
                  else
                    return false;
                }
                if(question.Contains("졸업논문") || question.Contains("졸업시험"))
                {
                  // 졸업시험, 논문 여부 미구현 -> 종설 들었거나 IPP 패스했다면 룰 PASS로 우회
                  // if(userInfo.graduationPaper == "O" || userInfo.graduationTest == "O")
                  IEnumerable<UserSubject> matches = userInfo.majorClasses.Where(
                                                      subject => subject.className.Contains("종합설계") 
                                                              || subject.className.Contains("현장실습"));
                  if(matches.Count() > 0)
                    return true;
                  else
                    return false;
                }
                break;
            case 2: // 최소한 하나 만족
                foreach (UserSubject userClass in rule.userClasses)
                {
                    foreach (Class reqClass in rule.requiredClasses)
                    {
                        if (userClass.classCode == reqClass.classCode)
                        {
                            return true;
                        }
                    }
                }
                break;
            case 3: // 전체 만족
                int count = 0;
                foreach (UserSubject userClass in rule.userClasses)
                {
                    foreach (Class reqClass in rule.requiredClasses)
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

      public void CheckRule()
      {
        if(this.rule == null)
          return;
        this.rule.isPassed = GetRuleChecked();
      }
    }



    public class Pair
    {
        public string year { get; set; }
        public string classCode { get; set; }

        public string retake { get; set; }

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

        public string retake { get; set; }
    }
    public class UserInfo
    {
        public string applicationYear { get; set; }//교육과정 적용년도
        public string advancedStatus { get; set; }//심화대상 여부
        public string englishTrack { get; set; }//영어트랙 여부
        public string university { get; set; }//단과대
        public string major { get; set; }//학과

        public string previousMajor { get; set; }//전과
        public string studentId { get; set; }//학번

        public string sudentName { get; set; }//이름
        public string minor1 { get; set; }//부전공1
        public string minor2 { get; set; }//부전공2
        public string doubleMajor1 { get; set; }//복수전공1
        public string doubleMajor2 { get; set; }//복수전공2

        public string[] englishPass { get; set; }//영어 패스 대상, 패스여부
        public string[] englishClassPass {get; set;} // 영어 강의 여부

        public string teaching { get; set; }//교직인적성 대상 여부

        public string graduationPaper {get;set;} // 졸업논문대상 ; 공대생 input file에 값이 없음
        public string graduationTest {get; set;} // 졸업시험대상 ; 공대생 input file에 값이 없음
 
        public int publicLibCredit { get; set; }
        public int basicLibCredit { get; set; }

        public int majorCredit { get; set; }
        public int majorDesignCredit { get; set; }

        public int majorEssentialCredit { get; set; }
        public int majorSpecialCredit { get; set; }

        public int mscCredit { get; set; }
        public int mscMathCredit { get; set; }
        public int mscScienceCredit { get; set; }
        public int mscScienceExperimentCredit { get; set; }
        public int mscComputerCredit { get; set; }

        public int englishCredit { get; set; }
        public int englishMajorCredit { get; set; }

        public int totalCredit { get; set; }
        public double gradeAverage {get; set;}

        public List<UserSubject> publicClasses = new List<UserSubject>();//기초교양 수강 목록
        public List<UserSubject> basicClasses = new List<UserSubject>();//기본소양 수강 목록
        public List<UserSubject> mscClasses = new List<UserSubject>();//MSC 수강 목록
        public List<UserSubject> majorClasses = new List<UserSubject>();//전공 수강 목록
        public List<UserSubject> majorEssentialList = new List<UserSubject>();//전공필수 수강 목록
        public List<UserSubject> majorDesignList = new List<UserSubject>();//전공설계 수강 목록
        public List<UserSubject> englishList = new List<UserSubject>();//영어강의 수강 목록
        public List<UserSubject> englishMajorList = new List<UserSubject>();//영어 전공강의 수강 목록

        public List<Pair> basicClassesPair = new List<Pair>();

        public void GetUserSubjects(List<UserSubject> userSubject_)
        {
            this.publicLibCredit = 0;
            this.basicLibCredit = 0;

            this.majorCredit = 0;
            this.majorSpecialCredit = 0;
            this.majorEssentialCredit = 0;
            this.majorDesignCredit = 0;

            this.mscCredit = 0;

            this.mscMathCredit = 0;
            this.mscScienceCredit = 0;
            this.mscScienceExperimentCredit = 0;
            this.mscComputerCredit = 0;

            this.englishCredit = 0;
            this.englishMajorCredit = 0;

            this.totalCredit = 0;
            this.gradeAverage = 0;

            foreach (UserSubject userSubject in userSubject_)
            {
                int subjectCredit = Convert.ToInt32(userSubject.credit);
                this.totalCredit += subjectCredit;

                if (userSubject.engineeringFactorDetail == "기초교양(교필)")
                {
                    this.publicLibCredit += subjectCredit;
                    this.publicClasses.Add(userSubject);
                }
                if (userSubject.engineeringFactorDetail == "기본소양")
                {
                    this.basicLibCredit += subjectCredit;
                    this.basicClasses.Add(userSubject);
                    //this.basicClassesPair.Add(new Pair
                    //{
                    //    year = userSubject.year,
                    //    classCode = userSubject.classCode,
                    //    retake = userSubject.retake
                    //});
                }
                if (userSubject.engineeringFactor == "MSC/BSM")
                {
                    this.mscCredit += subjectCredit;
                    switch (userSubject.engineeringFactorDetail)
                    {
                        case "수학":
                            this.mscMathCredit += subjectCredit;
                            break;
                        case "기초과학":
                            if (userSubject.className.Contains("실험"))
                                this.mscScienceExperimentCredit += subjectCredit;
                            this.mscScienceCredit += subjectCredit;
                            break;
                        case "전산학":
                            this.mscComputerCredit += subjectCredit;
                            break;
                        default:
                            break;
                    }
                    this.mscClasses.Add(userSubject);
                }
                if (userSubject.engineeringFactor == "전공")
                {
                    this.majorCredit += subjectCredit;
                    if (userSubject.completionDiv == "전필")
                    {
                        this.majorEssentialList.Add(userSubject);
                        this.majorEssentialCredit += subjectCredit;
                    }
                    if (userSubject.completionDivField == "전문")
                    {
                        this.majorSpecialCredit += subjectCredit;
                    }
                    if (userSubject.engineeringFactorDetail == "전공설계")
                    {
                        this.majorDesignCredit += subjectCredit;
                        this.majorClasses.Add(userSubject);
                        continue;
                    }
                    if (userSubject.english == "영어")
                    {
                        this.englishMajorCredit += subjectCredit;
                        this.englishMajorList.Add(userSubject);

                    }
                    this.majorClasses.Add(userSubject);
                }
                if (userSubject.english == "영어")
                {
                    this.englishCredit += subjectCredit;
                    this.englishList.Add(userSubject);
                }
            }
            CheckException();
            Console.WriteLine(this.basicLibCredit);
        }

        public void GetUserInfo(string filename_)
        {
            using (var infoStream = System.IO.File.Open(filename_, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var infoReader = ExcelReaderFactory.CreateReader(infoStream))
                {
                    int colNum = infoReader.FieldCount;

                    for (int i = 0; i < 3; i++)
                    { infoReader.Read(); }
                    infoReader.Read();
                    string[] split = new string[2];
                    string readCell = "";
                    for (int i = 0; i < colNum; i++)
                    {
                        if (infoReader.GetValue(i) != null)
                            readCell = infoReader.GetValue(i).ToString();

                        if (readCell.Contains("교육과정 적용년도"))
                        {
                            split = readCell.Split(":");
                            this.applicationYear = split[1].Trim();
                        }
                        if (readCell.Contains("공학인증심화대상"))
                        {
                            split = readCell.Split(":");
                            this.advancedStatus = split[1].Trim();
                        }
                        if (readCell.Contains("영어트랙여부"))
                        {
                            split = readCell.Split(":");
                            this.englishTrack = split[1].Trim();
                        }
                        if (readCell.Contains("심화과정 여부"))
                        {
                            split = readCell.Split(":");
                            this.advancedStatus = split[1].Trim();
                        }
                        
                    }
                    infoReader.Read();
                    for (int i = 0; i < colNum; i++)
                    {
                        if (readCell.Contains("대학"))
                        {
                            //split = readCell.Split(":");
                            this.university = infoReader.GetValue(i + 1).ToString().Trim();
                        }
                        if (readCell.Contains("전과"))
                        {
                            split = readCell.Split(":");
                            this.previousMajor = split[1].Trim();
                        }
                    }

                    infoReader.Read();
                    for (int i = 0; i < colNum; i++)
                    {
                        if (readCell.Contains("학과"))
                        {
                            //split = readCell.Split(":");
                            this.major = infoReader.GetValue(i + 1).ToString().Trim();
                        }
                        if (readCell.Contains("학번"))
                        {
                            split = readCell.Split(":");
                            this.previousMajor = split[1].Trim();
                        }
                        if (readCell.Contains("성명"))
                        {
                            split = readCell.Split(":");
                            this.previousMajor = split[1].Trim();
                        }
                        if (readCell.Contains("부전공1"))
                        {
                            split = readCell.Split(":");
                            this.minor1 = split[1].Trim();
                        }
                        if (readCell.Contains("부전공2"))
                        {
                            split = readCell.Split(":");
                            this.minor2 = split[1].Trim();
                        }
                        if (readCell.Contains("복수1"))
                        {
                            split = readCell.Split(":");
                            this.doubleMajor1 = split[1].Trim();
                        }
                        if (readCell.Contains("복수2"))
                        {
                            split = readCell.Split(":");
                            this.doubleMajor2 = split[1].Trim();
                        }
                    }

                    while (infoReader.Read())
                    {
                        for (int i = 0; i < colNum; i++)
                        {
                            readCell = "";
                            //if (infoReader.GetValue(i) == null)
                            //    readCell = "";
                            //else
                            if (infoReader.GetValue(i) != null)
                                readCell = infoReader.GetValue(i).ToString();

                            if (readCell.Contains("영어패스제"))
                            {
                                split = infoReader.GetValue(i).ToString().Split(":");
                                this.englishPass = split[1].Split(",");
                                if(englishPass.Length > 1)
                                {
                                  englishPass[0] = englishPass[0].Trim();
                                  // Console.WriteLine(englishPass[0]);
                                  if (englishPass[0] == "대상")
                                      englishPass[1] = englishPass[1].Trim();
                                  else
                                      englishPass[1] = "";
                                }

                                // Console.WriteLine(englishPass[1]);
                            }
                            if(readCell.Contains("영어강의이수:")) // :붙이는 이유: 다음에 오는 '영어강의이수결과' 때문
                            {
                                split = infoReader.GetValue(i).ToString().Split(":");
                                this.englishClassPass = split[1].Split(",");
                                if(englishClassPass.Length > 1)
                                {
                                  englishClassPass[0] = englishClassPass[0].Trim();
                                  // Console.WriteLine(englishPass[0]);
                                  if (englishClassPass[0] == "대상")
                                      englishClassPass[1] = englishClassPass[1].Trim();
                                  else
                                      englishClassPass[1] = "";
                                }
                            }
                            if(readCell.Contains("평점평균"))
                            {
                              split = infoReader.GetValue(i).ToString().Split(":");
                              this.gradeAverage = Convert.ToDouble(split[1]);
                            }
                            if (readCell.Contains("교직"))
                            {
                                split = infoReader.GetValue(i).ToString().Split(":");
                                this.teaching = split[1];
                                this.teaching = teaching.Trim();
                            }
                        }
                    }
                }
            }
        }
        // 공학경제, 공학법제, 지속가능, 기술과사회
        public string[] basicArray = new string[] { "PRI4041", "PRI4043", "PRI4048", "PRI4040" };
        // 동일교과 추가해야함
        public void CheckException()
        {
            List<UserSubject> temp = basicClasses;
            foreach (string basicArray_ in basicArray)
            {
                foreach (UserSubject basicClassesPair_ in temp)
                {
                    if (basicArray_ == basicClassesPair_.classCode)//예외 처리할 과목명 일치시
                    {
                        if (Convert.ToInt32(basicClassesPair_.year) >= 2021)// 수강년도가 2021년 이후
                        {
                            if (basicClassesPair_.retake != "NEW재수강")//재수강이 아닐경우
                            {
                                this.basicClasses.Remove(new UserSubject() { classCode = basicClassesPair_.classCode });
                                this.basicLibCredit -= Convert.ToInt32(basicClassesPair_.credit);
                            }
                        }
                    }
                }

            }
        }

    }
}
