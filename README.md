## CodeExercise Documentation

## Setup
1.	Download the CodeExercise.rar from Email
2.	Unzip the RAR file and open the CodeExercise.sln
3.	Build the application and set the CodeExercise project as startup project.
4.	Run the solution and use any browser or REST client to execute the APIs

## General Information
The project is built with ASP.NET Web API 2. The project also follows SOLID principle as much as possible.

## APIs to Execute
{URL}/api/Payment/WhatsYourId

{URL}/api/Payment/IsCardNumberValid?cardNumber=

{URL}/api/Payment/IsValidPaymentAmount?amount=

{URL}/api/Payment/CanMakePaymentWithCard?cardNumber=&expiryMonth=&expiryYear

## Tools used
Ninject – Dependency Injection

NUnit – Unit Test


I have made sure that the project uses the Nuget all the time.
