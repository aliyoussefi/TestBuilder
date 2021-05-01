using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using PowerAutomate.PackageManager.Models;

namespace PowerAutomate.PackageManager {

    public class PowerAutomatePackageManager {
        private Dictionary<string, string> BubbleSort(Dictionary<string, string> runOrder, Dictionary<string, string> runActions) {
            //int num = actions.Count;
            //for (int i = 0; i < num - 1; i++)
            //    for (int j = 0; j < num - i - 1; j++)
            //        if (actions[j] > actions[j + 1]) {
            //            // swap tmp and arr[i] int tmp = arr[j];
            //            actions[j] = actions[j + 1];
            //            actions[j + 1] = tmp;
            //        }
            Dictionary<string, string> sortedOrder = new Dictionary<string, string>();
            Dictionary<string, string> rtnObject = new Dictionary<string, string>();
            foreach (var action in runOrder) {
                if (action.Key == "[FIRST]") {
                    sortedOrder.Add(action.Key, action.Value);
                    runOrder.Remove(action.Key);
                    var nextActionAfterInitialize = runOrder.Where(x => x.Key == action.Value).First();
                    sortedOrder.Add(nextActionAfterInitialize.Key, nextActionAfterInitialize.Value);
                    runOrder.Remove(nextActionAfterInitialize.Key);
                }
                var nextAction = runOrder.Where(x => x.Key == sortedOrder.Last().Value).FirstOrDefault();
                if (nextAction.Key != null) {
                    sortedOrder.Add(nextAction.Key, nextAction.Value);
                    runOrder.Remove(nextAction.Key);
                }
            }
            foreach (var item in sortedOrder) {
                rtnObject.Add(item.Value, runActions[item.Value]);
            }



            return rtnObject;
        }



        public StringBuilder BuildTest() {
            Console.WriteLine("Hello World!");
            //if (args.Length == 0) {
            //    System.Console.WriteLine("Please enter a file.");
            //}
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/main-and-command-args/command-line-arguments#:~:text=The%20following%20example%20shows%20how%20to%20use%20command-line,that%20explains%20the%20correct%20usage%20of%20the%20program.
            string fileName = @"C:\Users\alyousse\source\repos\PowerAutomate.PackageManager\OpenAccountAndAssertNameIsTest-8210B307-01AA-EB11-B1AC-000D3A32AD05.json";
            string content = string.Empty;
            //"C:\Users\alyousse\source\repos\PowerAutomate.PackageManager\MyFirstUiTest-594CB561-95A2-EB11-B1AC-000D3A3B7D71.json"
            using (var streamReader = new StreamReader(fileName, Encoding.UTF8)) {
                content = streamReader.ReadToEnd();
            }
            PowerAutomate.PackageManager.Models.PowerAutomateFlow flow = Newtonsoft.Json.JsonConvert.DeserializeObject<PowerAutomateFlow>(content);
            var test = JObject.Parse(content);
            var actions = test.SelectToken("properties").SelectToken("definition").SelectToken("actions");
            Dictionary<string, string> runOrder = new Dictionary<string, string>();
            Dictionary<string, string> runActions = new Dictionary<string, string>();
            foreach (var action in actions.Children()) {

                if (action.Path.Contains("Initialize_Browser")) {
                    var initializeBrowser = action.First["inputs"]["parameters"].ToObject<InitializeBrowser>();
                    initializeBrowser.runAfter = new RunAfter()
                    {
                        name = (action.First["runAfter"].HasValues) ? ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name : "[FIRST]"
                    };

                    initializeBrowser.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(initializeBrowser.runAfter.name, initializeBrowser.type);
                    runActions.Add(initializeBrowser.type, @"var client = new WebClient(TestSettings.Options);
                                        using (var xrmApp = new XrmApp(client)) {
                                    ");
                    Console.WriteLine("Added " + initializeBrowser.type + " to run after " + initializeBrowser.runAfter.name);
                }
                if (action.Path.Contains("Open_App")) {
                    var openApp = action.First["inputs"]["parameters"].ToObject<OpenApp>();
                    openApp.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    openApp.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(openApp.runAfter.name, openApp.type);
                    runActions.Add(openApp.type, String.Format(@"xrmApp.Navigation.OpenApp(""{0}"");", openApp.name));
                    Console.WriteLine("Added " + openApp.type + " to run after " + openApp.runAfter.name);
                }
                if (action.Path.Contains("Open_SubArea")) {
                    var subArea = action.First["inputs"]["parameters"].ToObject<OpenSubArea>();
                    subArea.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    subArea.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(subArea.runAfter.name, subArea.type);
                    runActions.Add(subArea.type, String.Format(@"xrmApp.Navigation.OpenSubArea(""{0}"", ""{1}"");", subArea.area, subArea.subArea));
                    Console.WriteLine("Added " + subArea.type + " to run after " + subArea.runAfter.name);
                }
                if (action.Path.Contains("Login")) {
                    //Console.WriteLine("xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);");
                    var login = action.First["inputs"]["parameters"].ToObject<Login>();
                    login.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    login.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(login.runAfter.name, login.type);
                    runActions.Add(login.type, String.Format(@"xrmApp.OnlineLogin.Login(_xrmUri, _username.ToSecureString(), _password.ToSecureString());", login.uri, login.username, login.password));
                    Console.WriteLine("Added " + login.type + " to run after " + login.runAfter.name);
                }
                if (action.Path.Contains("Open_Record")) {
                    //Console.WriteLine("xrmApp.Grid.OpenRecord(0);");
                    var openRecord = action.First["inputs"]["parameters"].ToObject<OpenRecord>();
                    openRecord.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    openRecord.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(openRecord.runAfter.name, openRecord.type);
                    runActions.Add(openRecord.type, String.Format(@"xrmApp.Grid.OpenRecord({0});", openRecord.viewKey));
                    Console.WriteLine("Added " + openRecord.type + " to run after " + openRecord.runAfter.name);
                }
                if (action.Path.Contains("Set_Value")) {
                    //Console.WriteLine("xrmApp.Grid.OpenRecord(0);");
                    var setValue = action.First["inputs"]["parameters"].ToObject<SetValue>();
                    setValue.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    setValue.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(setValue.runAfter.name, setValue.type);
                    runActions.Add(setValue.type, String.Format(@"xrmApp.Entity.SetValue(""{0}"", ""{1}"");", setValue.field, setValue.value));
                    Console.WriteLine("Added " + setValue.type + " to run after " + setValue.runAfter.name);
                }
                if (action.Path.Contains("Assert")) {
                    //Console.WriteLine("xrmApp.Grid.OpenRecord(0);");
                    var assert = action.First["inputs"]["parameters"].ToObject<Assert>();
                    assert.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    assert.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(assert.runAfter.name, assert.type);
                    runActions.Add(assert.type, String.Format(@"string {0}value = xrmApp.Entity.GetValue(""{0}"");
                        Assert.AreEqual({0}value, ""{1}"", ""{2}"");", assert.field, assert.value, assert.errorMessage));
                    Console.WriteLine("Added " + assert.type + " to run after " + assert.runAfter.name);
                }
                if (action.Path.Contains("Condition")) {
                    //Console.WriteLine("xrmApp.Grid.OpenRecord(0);");
                    StringBuilder conditionBuilder = new StringBuilder();
                    //conditionBuilder.AppendLine(String.Format("if (true){}"));

                }
                if (action.Path.Contains("End")) {
                    //Console.WriteLine(action.ToString());
                    //Console.WriteLine(@"}
                    //    }");
                    var end = action.First["inputs"]["parameters"].ToObject<End>();
                    end.runAfter = new RunAfter()
                    {
                        name = ((Newtonsoft.Json.Linq.JProperty)action.First["runAfter"].First).Name
                    };
                    end.type = ((Newtonsoft.Json.Linq.JProperty)action).Name;
                    runOrder.Add(end.runAfter.name, end.type);
                    runActions.Add(end.type, @"}");
                    Console.WriteLine("Added " + end.type + " to run after " + end.runAfter.name);
                }
                //Console.WriteLine((JToken)action.);
            }

            runActions = BubbleSort(runOrder, runActions);
            StringBuilder easyReproCodeBuilder = new StringBuilder();
            foreach (var action in runActions) {
                
                easyReproCodeBuilder.AppendLine(action.Value);
            }
            easyReproCodeBuilder.AppendLine(@"}");
            Console.WriteLine(easyReproCodeBuilder.ToString());
            return easyReproCodeBuilder;
        }

    }


    [Generator]
    public class EasyReproGenerator : ISourceGenerator {
        public void Execute(GeneratorExecutionContext context) {
            // begin creating the source we'll inject into the users compilation
            StringBuilder easyReproTestBuild = new StringBuilder(@"
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using System.Security;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    [TestClass]
    public class OpenAccountUCI
    {
        private static string _username = String.Empty;
        private static string _password = String.Empty;
        private static BrowserType _browserType;
        private static Uri _xrmUri;

        public TestContext TestContext { get; set; }

        private static TestContext _testContext;

        [ClassInitialize]
        public static void Initialize(TestContext TestContext)
        {
            _testContext = TestContext;

            _username = _testContext.Properties[""OnlineUsername""].ToString();
            _password = _testContext.Properties[""OnlinePassword""].ToString();
            _xrmUri = new Uri(_testContext.Properties[""OnlineCrmUrl""].ToString());
            _browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _testContext.Properties[""BrowserType""].ToString());
        }

        [TestMethod]
        public void UCITestOpenActiveAccount()
        {


            ");
            PowerAutomatePackageManager powerAutomatePackageManager = new PowerAutomatePackageManager();

            easyReproTestBuild.AppendLine(powerAutomatePackageManager.BuildTest().ToString());
            easyReproTestBuild.AppendLine(@"        }
    }
}");

            
            StringBuilder sourceBuilder = new StringBuilder(@"
using System;
namespace HelloWorldGenerated
{
    public static class HelloWorld
    {
        public static void SayHello() 
        {
            Console.WriteLine(""Hello from generated code!"");
Console.WriteLine(""Hello from generated code!"");
            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");

");

            // using the context, get a list of syntax trees in the users compilation
            IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

            // add the filepath of each tree to the class we're building
            foreach (SyntaxTree tree in syntaxTrees) {
                sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
            }

            // finish creating the source to inject
            sourceBuilder.Append(@"
        }
    }
}");

            // inject the created source into the users compilation
            //context.AddSource("helloWorldGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

            context.AddSource("easyReproGenerated", SourceText.From(easyReproTestBuild.ToString(), Encoding.UTF8));

        }

        public void Initialize(GeneratorInitializationContext context) {
            // No initialization required
        }
    }
}
