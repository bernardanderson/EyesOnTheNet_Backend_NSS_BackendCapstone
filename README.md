## NSS Backend Capstone Part-1: "EyesOnTheNet Backend Web API"
#### (See the <a href="https://github.com/bernardanderson/EyesOnTheNet_Frontend_NSS_BackendCapstone">"Part 2"</a> for the Frontend Component)
 

### Specs:
> The idea behind this project was to build an application to allow secure access to webcameras which may have no or insecure authentication methods. A number of older webcameras are "out-in-the-wild" that have Authentication methods that are known to be easily hackable.  With 'EyesOnTheNet' a user can access webcameras via this more secure Backend Web API interface in order to still be able to access antiquated webcameras without having to put the actual webcameras on the public internet. This application was extended to allow for public webcamera access as well as displaying the physical location of the user entered camera via GoogleMaps. 
> This is the "Part 1" Backend portion of the project. See <a href="https://github.com/bernardanderson/EyesOnTheNet_Frontend_NSS_BackendCapstone">"Part 2"</a> for the Frontend user interface portion.

   
### Main Technologies Used:
> 1. DotNet Core (in Linux)
> 1. MySQL Server (in Linux)
> 1. Entity Framework/LINQ 
> 1. Json Web Tokens (for Authorization)
> 1. GoogleMaps (for Camera location information)
> 1. FileIO (for picture retrieval and transport)


### Final Result:
##### A secure WEB API backend interface allowing users access to web cams without compromising the security of the camera or the IP address.

> This Backend component allows a user access to web camera images via standard HTTP Requests.
> With this a user can:
> 1. Generate/Retrieve a Json web token for authentication
> 1. Manipulate a MySQL Database to store/retrieve/edit a personal list a public/private webcams.
> 1. Retrieve a single snapshot from one of the user's webcameras.
> 1. Retrieve a static Google Map image of a physical location for a webcam. 
> 1. NOTE: All webcam/GoogleMap traffic is done through the Backend API. The user sees none of the actual traffic to the webcameras. 

### How to run:
```
1. Install the <a href="https://www.microsoft.com/net/core">DotNet Core</a> package
2. Download/Clone this repo  
3. At the base directory run `dotnet restore` to store the required packages  
4. At the base directory run `dotnet run` to start the application
5. NOTE: For the full experience, the Frontend portion needs to be utilized to access   
```

### Specs By:
[Bernie Anderson](https://github.com/bernardanderson) 

### Contributors:
[Bernie Anderson](https://github.com/bernardanderson) 
