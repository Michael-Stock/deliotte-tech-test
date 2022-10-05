using CityWeatherApp.MsTestApi.Common;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace CityWeatherApp.MsTestApi.Steps
{
    [Binding]
    public sealed class TableSteps
    {
        private readonly ItemContext _itemContext;

        public TableSteps(ItemContext itemContext)
        {
            _itemContext = itemContext;
        }

        [StepDefinition(@"that ""(.*)"" contains the following properties")]
        public void GivenThatContainsTheFollowingProperties(string contextName, Table table)
        {
            var contextValue = _itemContext.JsonLookup[contextName];

            if (contextValue == null)
                throw new Exception($"Could not resolve {contextName} in the context");

            foreach (var row in table.Rows)
            {
                var propertyName = row.ElementAt(0).Value;
                var propertyValue = row.ElementAt(1).Value;

                var actual = contextValue.GetValue(propertyName);

                if (actual == null)
                    throw new Exception($"Could not find {propertyName} on the object");

                var actualValue = actual.ToString();
                actualValue.Should().Be(propertyValue);
            }
        }

        [StepDefinition(@"that ""(.*)"" contains the following instances")]
        public void GivenThatContainsTheFollowingInstances(string contextName, Table table)
        {
            var contextValue = GetByPropertyPath(_itemContext, contextName);

            var arrayObject = contextValue as JArray;

            if (arrayObject == null)
                throw new Exception($"Could not resolve {contextName} as an array");

            // First column is the match column
            var matchHeader = table.Header.ElementAt(0);

            foreach (var row in table.Rows)
            {
                var matchingEntry = arrayObject.Children().Where(c => c[matchHeader].ToString() == row.Values.ElementAt(0)).First();

                foreach (var header in table.Header)
                {
                    var expected = row[header];
                    var actual = matchingEntry[header].ToString();

                    actual.Should().Be(expected);
                }
            }
        }

        private JToken GetByPropertyPath(ItemContext itemContext, string propertyPath)
        {
            var parts = propertyPath.Split(".");

            var result = itemContext.JsonLookup[parts[0]] as JToken;

            foreach (var part in parts.Skip(1))
            {
                result = result[part];

                if (result == null)
                    throw new Exception($"Could not resolve {propertyPath} in the context");
            }

            return result;
        }
    }
}
