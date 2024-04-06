# Task Management app

## Introduction
- The Task Management Application is made to make one’s life easier, to be more productive and simplify the daily and day-to-day tasks for the user to complete them in time, so they can organize and track the activities of their lives. The application is integrated with advanced features with a user-friendly interface and UX, making task management an enjoyable experience. This README.md file provides you with a good look at the application functionalities, Architectural Foundation, and Detailed User Interactions.

## Motivation
- The inspiration behind this app was inspired by my everyday struggle with disorganized chaos. With an endless to-do list and reminders set for each, life's real interruptions most often mess up all planned follow-ups. Its creation goes beyond the aim of executing functions; it tries to create a better experience for the user corresponding to the unique demands of the tasks.

## Architectural Foundation: Embracing MVVM
- Model-View-ViewModel (MVVM) architecture is the core of the application, it is a very rigid framework that can be maintained and scalable:
- Model: This layer represents the application's backbone, including all data: user information, tasks, and details.
- View: The View is what the user sees. It is designed to be very clear, responsive, and intuitive so that there is absolutely no barrier between the user and interaction.
- ViewModel: Acts as an intermediate between Model and View; it is the one that handles the business logic and user input, ensuring the data provided is related in a bound way with the UI.

# Exploring the Features

## Engaging User Interface
- Splash Screen & App Logo: There is an attractive splash screen and app logo that allows the user to be welcomed, which, in turn, creates a good user experience.
- DateTimePicker: A custom DateTimePicker to provide awesome user interactions in date and time selection with ease and high efficiency.

## Task Management Capabilities
- Task Organization: The users can easily add, edit, or delete any task. The application organizes a very streamlined way in which the user can easily manage the tasks, thus increasing productivity.
- Location-Based Notifications: The app uses a geolocation feature that will assist the app display notifications for relevant tasks. Hence, the location area for the user is reminded of their tasks and thus can give the right task reminders according to location.

## Additional Functionalities
- Customizable Settings: The app comes with various settings in which users can change their kind of experience, from the kind of theme they want to their notification preference.
- Security Features: The app provides a more robust authentication system for the user during logging into their account, including enabling users to set their security configurations from within the application.

## Technical Infrastructure
- SQLite Database: For the systematic storage of information to ensure a quick and reliable system while accessing user and task details, the application uses an SQLite database.
- Service Layer Functionality: Contains a dedicated service layer that oversees the effective, efficient, and reliable backend operations for the application, including data handling, user authentication, and notification management.

## Detailed User Interactions
- Simplified Login Process: The app keeps a balance between highly secure means and easy user accessibility by simplifying the process for a login or new account creation.
- Homepage Dashboard: Users can manage their tasks right on the homepage through intuitive interactions.
- Adaptive Notifications: The application will use users' location to prioritize tasks and tailor messages accordingly, to improve the relevance and timeliness of tasks.
- Task History: it includes a detailed history of the tasks completed, which can help track progress and review activity.

## Geo-location Integration:
- The .NET Maui built-in Geolocation API has been used to achieve the geolocation capability functionality.
- This API is a part of the Microsoft.Maui.Devices namespace, which brings several other APIs that help in interacting with the device-specific features, such as geolocation services.
- Fetch the last known location using the methods GetLastKnownLocationAsync and GetLocationAsync inside the .NET MAUI Geolocation class. Continual tracking of location updates through the function StartLocationUpdatesAsync.
- 
# Summary
The Task Management App is here to revolutionize the game with which you deal with your daily tasks. Made for a more flowing day, this is a simple way to manage your tasks with just a few taps. With location-based reminder features, and most importantly, a safe and user-friendly interface, it's all about taking all the hassles away from staying on top of your life. Think of this application as your daily aid, ensuring that you stay on top of the tasks and get to enjoy doing them.







