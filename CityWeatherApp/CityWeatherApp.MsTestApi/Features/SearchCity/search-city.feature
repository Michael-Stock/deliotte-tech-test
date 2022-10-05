Feature: Searches a city based on name
	The search returns all cities that match the name

@search_city
Scenario: Search for a city by name returns an exact match
	
	Given a "AddCityRequest" loaded from "./SearchCity/Requests/AddNewYork.json"

	And that "AddCityRequest" contains the following properties
		| PropertyName        | Value      |
		| Name                | New York   |
		| State               | New York   |
		| TouristRating       | 5          |
		| Country             | USA        |
		| DateEstablished     | 2001-05-01 |
		| EstimatedPopulation | 23000      |

	And I POST that "AddCityRequest" to "/CityWeather" and store the "PostCityResponse"
	And the status code is 204

	And I GET "/CityWeather?name=New York" and store the "GetCityResponse"
	And the status code is 200

	And that "GetCityResponse.Cities" contains the following instances
		| Name     | State    | TouristRating | Country | EstimatedPopulation |
		| New York | New York | 5             | USA     | 23000               |