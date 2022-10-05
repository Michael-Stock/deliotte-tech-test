@search_city
Feature: Searches a city based on name
	The search returns all cities that match the name

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

		Scenario: Search for city by name returns multiple matches

			Given a "AddNewportUK" loaded from "./SearchCity/Requests/AddNewport_UK.json"

			And that "AddNewportUK" contains the following properties
				| PropertyName        | Value          |
				| Name                | Newport        |
				| State               | Gwent          |
				| TouristRating       | 2              |
				| Country             | United Kingdom |
				| DateEstablished     | 2001-05-01     |
				| EstimatedPopulation | 30000          |

			And I POST that "AddNewportUK" to "/CityWeather" and store the "PostCityResponseUK"
			And the status code is 204

			Given a "AddNewportUS" loaded from "./SearchCity/Requests/AddNewport_US.json"

			And that "AddNewportUS" contains the following properties
				| PropertyName        | Value        |
				| Name                | Newport      |
				| State               | Rhode Island |
				| TouristRating       | 5            |
				| Country             | USA          |
				| DateEstablished     | 2001-05-01   |
				| EstimatedPopulation | 50000        |

			And I POST that "AddNewportUS" to "/CityWeather" and store the "PostCityResponseUS"
			And the status code is 204

			And I GET "/CityWeather?name=Newport" and store the "GetCityResponse"
			And the status code is 200

			And that "GetCityResponse.Cities" contains the following instances
				| State        | Name    | TouristRating | Country        | EstimatedPopulation |
				| Gwent        | Newport | 2             | United Kingdom | 30000               |
				| Rhode Island | Newport | 5             | USA            | 50000               |