using CityWeatherApp.MsTestApi.Common;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace CityWeatherApp.MsTestApi.Steps
{
    [Binding]
    public sealed class AssertionSteps
    {
        private readonly ItemContext _itemContext;

        public AssertionSteps(ItemContext itemContext)
        {
            _itemContext = itemContext;
        }
    }
}
