# ValisSurveys
An ASP.NET (WebForms) survey system (working prototype).
This was a project for a client of mine arround year 2014. But the project canceled never went to production and abandoned. 
At the time I studied how SurveyMonkey and LimeSurvey were working and what were their mechanics (functionally wise). I borrowed from them the knowledge of the business domain. The system was near to complete its first version before the cancelation. 
I uploaded here today the solution, primarily as an educational resource. I revive it in order to be compiled under Visual Studio 2019 and upgraded it for .Net Framework 4.8. The databases scripts and the store procedures where developed for SQL Serve 2012. It runs perfectly on Firefox Browser and Google Chrome.

It supports plain surveys and surveys with branching logic. Each survey can have from one to multiple pages and each page can have one or more questions. A question can be any of the following types:

1. __SingleLine__ (One line of text input)
2. __MultipleLine__ (Multiple lines of text input)
3. __Integer__ (Numeric input)
4. __Decimal__ (Decimal input)
5. __Date__
8. __OneFromMany__ (List of radio buttons.Radio buttons indicate that respondents can only select one answer from the list of choices. When a respondent selects an answer choice, all other answer choices are deselected automatically. Closed-ended question type. Supports skip logic)
9. __ManyFromMany__ (List of checkboxes. Checkboxes indicate that respondents are allowed to select multiple answers from the list of choices. Closed-ended question type.)
10. __DropDown__ (DropDown control. Closed-ended question type. Supports skip logic)
13. __Range__
14. __MatrixOnePerRow__ (A matrix with radio controls)
15. __MatrixManyPerRow__ (A matrix with checkboxes)

THe system for every customer, supports contact goups and contact lists, automatically message creation for the invitations, schedulling etc.


## It consists of four (4) entities:
1. __ValisManager__
This is the backend of the system. It is the same for the administrators of the system and for the customers of the system. The admins can create new Customers and Accounts and Payments Profiles for them. The customers from here can create surveys, contact and contact groups, and schedule when invitations are to be send
2. __ValisServer__
This is the "RuntimeEngine" af the system. It has the purpose to render the surveys to client's devices and gather their responses.
3. __ValisApplicationService__
This is a windows service with the main responsibility to send the invitations (email) and all the other system emails (validations, subscrive, unsuscribe etc.). At the time I used the SendGrid service.
4. __ValisReporter__
This is the "ReportEngine" (analysis and report tool) for the survey's responses. It uses the excellent [HighCharts](https://www.highcharts.com/) to show the results for every questions (pies and barcharts) and the [phantomjs](https://phantomjs.org/) to produce a pdf report.

## Setup
1. Create an empty SQL Server database with the name ValisSurveys. Run the scipts inside the folder DatabaseScripts in the order of their numbering.
2. Create a folder somewhere where the system can put files and logs.
3. Open the three (3) Web.config and the one (1) App.config and change under <valisSystem> section the settings for the Database connection. Also change the FileInventory setting to show to the folder youcreated for the system's file. Same way change  the file setting for the log4Net.
4. Now you can run the UnitTests. The Unit Tests do not use mockup etc. They are more "integration tests" They hit the real database and they wait a clean system as it is after step 1!. Always if you want to run the UnitTests you must do it on a clean database.
5. After step 1 in the system there are the following accounts for your adnimistrators:
  1. user: sysadmin pass:tolk!3n
  2. user: developer pass:tolk!3n
  3. usewr: admin pass: tolk!3n
  
7. Be Careful. Only the  ValisManager is desinged to be used by the end users. The ValisServer and the ValisReporter are being called from the ValisManager or the invitation you send in order to collect responses to a survey. In order to view/test the whole sysem, the ValisServer and the ValisReporter must be running and their urls must be hardwritten in the web.configs. The corresponding settings are  <RuntimeEngine> and the <ReportEngine> inside the <valisSystem> section.
6. Login in the ValisManager, create a client using the demo-payment-profile (the easiest) and create a user. Then login as this user. Now you can create surveys, preview these surveys, contacts, etc..
8. Don't forget to check the license model for [HighCharts](https://www.highcharts.com/) as it is not a free javascript charts library.


## Synopsis
I hope this project to fulfill its educational purpose or to be of some use to other people. Let me know.
