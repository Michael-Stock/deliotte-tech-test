﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace CityWeatherApp.MsTestApi.Features.SearchCity
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class SearchesACityBasedOnNameFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
        private string[] _featureTags = new string[] {
                "search_city"};
        
#line 1 "search-city.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features/SearchCity", "Searches a city based on name", "\tThe search returns all cities that match the name", ProgrammingLanguage.CSharp, new string[] {
                        "search_city"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Searches a city based on name")))
            {
                global::CityWeatherApp.MsTestApi.Features.SearchCity.SearchesACityBasedOnNameFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Search for a city by name returns an exact match")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Searches a city based on name")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("search_city")]
        public virtual void SearchForACityByNameReturnsAnExactMatch()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for a city by name returns an exact match", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 5
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 7
   testRunner.Given("a \"AddCityRequest\" loaded from \"./SearchCity/Requests/AddNewYork.json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "PropertyName",
                            "Value"});
                table1.AddRow(new string[] {
                            "Name",
                            "New York"});
                table1.AddRow(new string[] {
                            "State",
                            "New York"});
                table1.AddRow(new string[] {
                            "TouristRating",
                            "5"});
                table1.AddRow(new string[] {
                            "Country",
                            "USA"});
                table1.AddRow(new string[] {
                            "DateEstablished",
                            "2001-05-01"});
                table1.AddRow(new string[] {
                            "EstimatedPopulation",
                            "23000"});
#line 9
   testRunner.And("that \"AddCityRequest\" contains the following properties", ((string)(null)), table1, "And ");
#line hidden
#line 18
   testRunner.And("I POST that \"AddCityRequest\" to \"/CityWeather\" and store the \"PostCityResponse\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 19
   testRunner.And("the status code is 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 21
   testRunner.And("I GET \"/CityWeather?name=New York\" and store the \"GetCityResponse\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 22
   testRunner.And("the status code is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "Name",
                            "State",
                            "TouristRating",
                            "Country",
                            "EstimatedPopulation"});
                table2.AddRow(new string[] {
                            "New York",
                            "New York",
                            "5",
                            "USA",
                            "23000"});
#line 24
   testRunner.And("that \"GetCityResponse.Cities\" contains the following instances", ((string)(null)), table2, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Search for city by name returns multiple matches")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Searches a city based on name")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("search_city")]
        public virtual void SearchForCityByNameReturnsMultipleMatches()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Search for city by name returns multiple matches", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 28
  this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 30
   testRunner.Given("a \"AddNewportUK\" loaded from \"./SearchCity/Requests/AddNewport_UK.json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "PropertyName",
                            "Value"});
                table3.AddRow(new string[] {
                            "Name",
                            "Newport"});
                table3.AddRow(new string[] {
                            "State",
                            "Gwent"});
                table3.AddRow(new string[] {
                            "TouristRating",
                            "2"});
                table3.AddRow(new string[] {
                            "Country",
                            "United Kingdom"});
                table3.AddRow(new string[] {
                            "DateEstablished",
                            "2001-05-01"});
                table3.AddRow(new string[] {
                            "EstimatedPopulation",
                            "30000"});
#line 32
   testRunner.And("that \"AddNewportUK\" contains the following properties", ((string)(null)), table3, "And ");
#line hidden
#line 41
   testRunner.And("I POST that \"AddNewportUK\" to \"/CityWeather\" and store the \"PostCityResponseUK\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 42
   testRunner.And("the status code is 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 44
   testRunner.Given("a \"AddNewportUS\" loaded from \"./SearchCity/Requests/AddNewport_US.json\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "PropertyName",
                            "Value"});
                table4.AddRow(new string[] {
                            "Name",
                            "Newport"});
                table4.AddRow(new string[] {
                            "State",
                            "Rhode Island"});
                table4.AddRow(new string[] {
                            "TouristRating",
                            "5"});
                table4.AddRow(new string[] {
                            "Country",
                            "USA"});
                table4.AddRow(new string[] {
                            "DateEstablished",
                            "2001-05-01"});
                table4.AddRow(new string[] {
                            "EstimatedPopulation",
                            "50000"});
#line 46
   testRunner.And("that \"AddNewportUS\" contains the following properties", ((string)(null)), table4, "And ");
#line hidden
#line 55
   testRunner.And("I POST that \"AddNewportUS\" to \"/CityWeather\" and store the \"PostCityResponseUS\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 56
   testRunner.And("the status code is 204", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
   testRunner.And("I GET \"/CityWeather?name=Newport\" and store the \"GetCityResponse\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 59
   testRunner.And("the status code is 200", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "State",
                            "Name",
                            "TouristRating",
                            "Country",
                            "EstimatedPopulation"});
                table5.AddRow(new string[] {
                            "Gwent",
                            "Newport",
                            "2",
                            "United Kingdom",
                            "30000"});
                table5.AddRow(new string[] {
                            "Rhode Island",
                            "Newport",
                            "5",
                            "USA",
                            "50000"});
#line 61
   testRunner.And("that \"GetCityResponse.Cities\" contains the following instances", ((string)(null)), table5, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
