#pragma checksum "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6efda1bef9904a632c78d9a1bfb9af45053448e7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_Index), @"mvc.1.0.view", @"/Views/User/Index.cshtml")]
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
#line 1 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\_ViewImports.cshtml"
using TestProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\_ViewImports.cshtml"
using TestProject.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
using ReadExcel.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6efda1bef9904a632c78d9a1bfb9af45053448e7", @"/Views/User/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b5793b3861753add5f6e734f58173abd93064819", @"/Views/_ViewImports.cshtml")]
    public class Views_User_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Tuple<IEnumerable<UserModel>, IEnumerable<ReadExcel.Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>>
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
#line 4 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
   
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6efda1bef9904a632c78d9a1bfb9af45053448e73784", async() => {
                WriteLiteral("\r\n       <meta name=\"viewport\" content=\"width=device-width\" />\r\n    <title>Users</title>\r\n");
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
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6efda1bef9904a632c78d9a1bfb9af45053448e74846", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 15 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     using (Html.BeginForm("start", "user", FormMethod.Get))
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <input type=\"submit\" value=\"Go Back\" />\r\n");
#nullable restore
#line 18 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item1.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"        <hr />
        <table cellpadding=""0"" cellspacing=""0"" border=""1"">
            <tr>
                <th>구분</th>
                <th>일련번호</th>
                <th>질문</th>
                <th>응답유형</th>
                <th>비고</th>
            </tr>
");
#nullable restore
#line 30 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (UserModel user in Model.Item1)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 33 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(user.A);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 34 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(user.B);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 35 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(user.C);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 36 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(user.D);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 37 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(user.E);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>-\r\n                </tr>\r\n");
#nullable restore
#line 39 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 41 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 42 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item2.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <hr />\r\n        <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\r\n            <tr>\r\n                <th>학수번호</th>\r\n                <th>과목명</th>\r\n                <th>학점</th>\r\n                <th>연도</th>\r\n            </tr>\r\n");
#nullable restore
#line 52 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (ReadExcel.Models.Math class_ in Model.Item2)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 55 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(class_.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 56 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(class_.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 57 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(class_.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 58 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(class_.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 60 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 62 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 63 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item3.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <hr />\r\n        <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\r\n            <tr>\r\n                <th>학수번호</th>\r\n                <th>과목명</th>\r\n                <th>학점</th>\r\n                <th>연도</th>\r\n            </tr>\r\n");
#nullable restore
#line 73 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (BasicLiberalArts basicLiberal in Model.Item3)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 76 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicLiberal.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 77 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicLiberal.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 78 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicLiberal.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 79 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicLiberal.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 81 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 83 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 84 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item4.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <hr />\r\n        <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\r\n            <tr>\r\n                <th>학수번호</th>\r\n                <th>과목명</th>\r\n                <th>학점</th>\r\n                <th>연도</th>\r\n            </tr>\r\n");
#nullable restore
#line 94 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (BasicKnowledge basicKnowledge in Model.Item4)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 97 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicKnowledge.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 98 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicKnowledge.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 99 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicKnowledge.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 100 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(basicKnowledge.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 102 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 104 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 105 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item5.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <hr />\r\n        <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\r\n            <tr>\r\n                <th>학수번호</th>\r\n                <th>과목명</th>\r\n                <th>학점</th>\r\n                <th>연도</th>\r\n            </tr>\r\n");
#nullable restore
#line 115 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (ScienceExperiment scienceExperiment in Model.Item5)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 118 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(scienceExperiment.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 119 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(scienceExperiment.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 120 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(scienceExperiment.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 121 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(scienceExperiment.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 123 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 125 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 126 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item6.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral("        <hr />\r\n        <table cellpadding=\"0\" cellspacing=\"0\" border=\"1\">\r\n            <tr>\r\n                <th>학수번호</th>\r\n                <th>과목명</th>\r\n                <th>학점</th>\r\n                <th>연도</th>\r\n            </tr>\r\n");
#nullable restore
#line 136 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (MSC msc in Model.Item6)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 139 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(msc.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 140 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(msc.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 141 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(msc.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 142 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(msc.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 144 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 146 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 147 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
     if (Model.Item7.Count() > 0)
    {

#line default
#line hidden
#nullable disable
                WriteLiteral(@"        <hr />
        <table cellpadding=""0"" cellspacing=""0"" border=""1"">
            <tr>
                <th>학수번호</th>
                <th>과목명</th>
                <th>학점</th>
                <th>연도</th>
                <th>설계학점여부</th>
            </tr>
");
#nullable restore
#line 158 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
             foreach (MajorRequired majorRequired in Model.Item7)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 161 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(majorRequired.class_num);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 162 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(majorRequired.class_name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 163 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(majorRequired.credit);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 164 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(majorRequired.year);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 165 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
                   Write(majorRequired.project);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 167 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </table>\r\n");
#nullable restore
#line 169 "C:\Users\정동구\Downloads\TestProject\TestProject\Views\User\Index.cshtml"
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
            WriteLiteral("\r\n</html>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Tuple<IEnumerable<UserModel>, IEnumerable<ReadExcel.Models.Math>, IEnumerable<BasicLiberalArts>,
                IEnumerable<BasicKnowledge>, IEnumerable<ScienceExperiment>, IEnumerable<MSC>, IEnumerable<MajorRequired>>> Html { get; private set; }
    }
}
#pragma warning restore 1591
