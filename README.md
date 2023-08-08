# Traditional Handicraft Gallery

## Getting Started

1. Double click the `.sln` file to open the project.
2. Create a new file called `appsettings.Development.json`.
3. Copy the content in `appsettings.template.json` to the newly created `appsettings.Development.json`.
4. Modify any necessary settings in the `appsettings.Development.json`.
5. Go to Package Manager Console (at the bottom). Run `update-database` command.
6. Should be good to go.

## To go to admin page

1. Head to `/admin`.
2. Login with the credential `admin` & `Admin@123`.

## User's credential

1. `user` & `User@123`

## To deploy to Elastic Beanstalk

1. Use t2.micro
2. After successfully launched the Elastic Beanstalk in Visual Studio, go to AWS Web Interface.
3. Navigate to the Elastic Beanstalk dashboard.
4. Select the environment you want to inspect.
5. In the sidebar, select "Configuration".
6. Click the Edit button at the section "Updates, monitoring, and logging".
7. Scroll down to add new Environment properties with the key `ASPNETCORE_ENVIRONMENT` and value `Production`.
8. This will set the application to Production mode and will not rely on the AWS Credentials in appsettings file.

## What is ASP.NET Core MVC?
1. **Controller**:
   - **What it is**: Think of the Controller as a manager in a restaurant. It takes requests (orders) from customers and directs the kitchen (Models) and waitstaff (Views) to fulfill the request.
   - **What it does**: When a user interacts with the web application (clicking a link, submitting a form, etc.), the Controller is responsible for handling that request. It works with Models to get the needed data and selects the appropriate View to display the response.

2. **Models**:
   - **What it is**: Models are like recipes in a kitchen. They define the ingredients (data) and how they are combined to create a dish (information).
   - **What it does**: Models represent the data and the business logic of the application. They interact with databases to retrieve, manipulate, and store information. They define the structure of the data, like how a recipe defines what goes into a dish.

3. **Views**:
   - **What it is**: Views are like the presentation of the dish in a restaurant, how it's plated and looks to the customer.
   - **What it does**: Views are responsible for displaying data to the user. They define how the information is presented on the webpage, like HTML templates. A View might display a list of products, a form to submit information, or any other user interface.

4. **View Models**:
   - **What it is**: View Models are like a customized recipe card, tailored for a particular customer's taste or dietary need.
   - **What it does**: View Models are specialized models used to pass data from the Controller to the View. They often combine or simplify information from one or more Models to make it easier to display in a View. For example, if you want to show a summary of a user's profile and their order history on the same page, you might create a View Model that combines those details.

In summary, here's how they work together:
- The **Controller** receives a user's request and figures out what to do next.
- It talks to the **Models** to get or update the necessary data.
- It uses **View Models** to organize that data in a way that's easy to display.
- It sends the organized data to the **Views**, which display it to the user.

The entire flow is like taking an order, preparing a dish, and serving it to the customer in a restaurant. It's a structured way to build web applications, making it easier to understand and maintain.
