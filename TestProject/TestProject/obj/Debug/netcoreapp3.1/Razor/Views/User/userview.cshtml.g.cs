#pragma checksum "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "07b8c1cd2e0f6df8032b60347ffcd675f3ddaa70"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_userview), @"mvc.1.0.view", @"/Views/User/userview.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/_ViewImports.cshtml"
using TestProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/_ViewImports.cshtml"
using TestProject.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
using ReadExcel.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"07b8c1cd2e0f6df8032b60347ffcd675f3ddaa70", @"/Views/User/userview.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a67df7f6a57481d677f7e1dea8e90b584633d697", @"/Views/_ViewImports.cshtml")]
    public class Views_User_userview : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>>>
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
  
  Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<!DOCTYPE html>\n<html>\n\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "07b8c1cd2e0f6df8032b60347ffcd675f3ddaa703753", async() => {
                WriteLiteral("\n  <meta name=\"viewport\" content=\"width=device-width\" />\n  <title>UserView</title>\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "07b8c1cd2e0f6df8032b60347ffcd675f3ddaa704791", async() => {
                WriteLiteral("\n");
#nullable restore
#line 16 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
   using (Html.BeginForm("start", "User", FormMethod.Get))
  {

#line default
#line hidden
#nullable disable
                WriteLiteral("    <input type=\"submit\" value=\"Go Back\" />\n");
#nullable restore
#line 19 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
  }

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
   if (Model.Item1.Count() > 0)
  {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"    <hr />
    <table cellpadding=""0"" cellspacing=""0"" border=""1"">
      <tr>
        <th>년도</th>
        <th>학기</th>
        <th>이수구분</th>
        <th>이수구분영역</th>
        <th>학수번호</th>
        <th>교과목명</th>
        <th>학점</th>
        <th>공학요소</th>
        <th>공학세부요소</th>
        <th>원어강의종류</th>
      </tr>
");
#nullable restore
#line 36 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
       foreach (UserSubject user in Model.Item1)
      {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <tr>\n          <td>");
#nullable restore
#line 39 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 40 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.semester);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 41 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.completionDiv);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 42 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.completionDivField);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 43 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.classCode);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 44 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.className);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 45 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 46 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.engineeringFactor);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 47 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.engineeringFactorDetail);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n          <td>");
#nullable restore
#line 48 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
         Write(user.english);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n        </tr>\n");
#nullable restore
#line 50 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
      }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </table>\n");
#nullable restore
#line 52 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
  }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"  <hr />
  <table cellpadding=""0"" cellspacing=""0"" border=""1"">
    <tr>
      <th>공통교양</th>
      <th>기본소양</th>
      <th>msc</th>
      <th>수학</th>
      <th>과학</th>
      <th>전산학</th>
      <th>전공</th>
      <th>전필</th>
      <th>전문</th>
      <th>전공설계</th>
      <th>영어</th>
      <th>전공영어</th>
    </tr>
    <tr>
      <td>");
#nullable restore
#line 70 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.publicLibCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 71 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.basicLibCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 72 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.mscCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 73 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.mscMathCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 74 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.mscScienceCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 75 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.mscComputerCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 76 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.majorCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 77 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.majorEssentialCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 78 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.majorSpecialCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 79 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.majorDesignCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 80 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.englishCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n      <td>");
#nullable restore
#line 81 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
     Write(Model.Item2.englishMajorCredit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n    </tr>\n  </table>\n  <hr />\n");
                WriteLiteral("  <p>");
#nullable restore
#line 96 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
 Write(Model.Item3.Count());

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\n");
#nullable restore
#line 97 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
   if (Model.Item3.Count() > 0)
  {
    int passedRule = 0;

#line default
#line hidden
#nullable disable
                WriteLiteral("    <hr />\n    <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\n      <tr>\n        <th>번호</th>\n        <th>구분</th>\n        <th>질문</th>\n        <th>만족여부</th>\n      </tr>\n");
#nullable restore
#line 108 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
       foreach (Rule rule in Model.Item3)
      {
        if (5 < Convert.ToInt32(@rule.number) && Convert.ToInt32(@rule.number) < 32)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("          <tr>\n            <td>");
#nullable restore
#line 113 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
           Write(rule.number);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n            <td>");
#nullable restore
#line 114 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
           Write(rule.type);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n            <td>");
#nullable restore
#line 115 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
           Write(rule.question);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\n            <td>\n");
#nullable restore
#line 117 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
               if (Convert.ToInt32(@rule.number) > 5)
              {
                if (@rule.isPassed)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                  <center>O</center>\n");
#nullable restore
#line 122 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
                  passedRule += 1;
                }
                else
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                  <center><b>X</b></center>\n");
#nullable restore
#line 127 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
                }
              }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </td>\n          </tr>\n");
#nullable restore
#line 131 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
        }
      }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </table>\n    <hr />\n    <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\n      <tr>\n        <th>졸업가능여부</th>\n        <th>만족조건/전체조건</th>\n      </tr>\n      <tr>\n        <td>\n          <center>\n");
#nullable restore
#line 143 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
             if (passedRule < 27)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("              <b>Fail</b>\n");
#nullable restore
#line 146 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("              <b>Pass</b>\n");
#nullable restore
#line 150 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("          </center>\n        </td>\n\n        <td>");
#nullable restore
#line 154 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
       Write(passedRule);

#line default
#line hidden
#nullable disable
                WriteLiteral(" / 27</td>\n      </tr>\n    </table>\n");
#nullable restore
#line 157 "/Users/apple/Desktop/GraduationAssessment/GraduationAssesment/TestProject/TestProject/Views/User/userview.cshtml"
  }

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Tuple<IEnumerable<UserSubject>, UserInfo, List<Rule>>> Html { get; private set; }
    }
}
#pragma warning restore 1591
