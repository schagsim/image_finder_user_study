# Image Finder User Study
Simple web application for user study, letting users find a presented image in a gallery

## Technical details
### Technology
Image Finder is written in C# in .NET CORE 3.1.
This allows us to use RazorPages where we can directly call C# code from the .cshtml files.
Advantage of this technology is the simplicity of creating html pages.
The disadvantage is that it also bounds you to only use the .NET CORE tools in these html pages.
This can sometimes make your life harder compared to React/Typescript and Node.js implementation.

.NET CORE is built to be a framework independent package which can run anywhere where there's a .NET CORE runtime.

### Server details
Image Finder runs with Kestrel proxy in front of the web.
This doesn't mean it can't run at IIS. If we deploy it to IIS, IIS is going to act as a forwarder.

### Implementation
TODO

## Running the application
### Required files
First, you need to have
1) Google image labels files
2) Color histogram files
3) Semantic vectors files

By default, in appSettings.json, these file paths are set to
1) "Resources/GoogleLabelsFiles"
2) "Resources/ColorHistogramFiles"
3) "Resources/SemanticVectorsFiles"
respectively. However, you can change these file paths.

## Running Python scripts
### Histogram generation
Go to folder `./python_scripts`. Create and activate a virtual environment.
```
python3 -m venv ./venv
source venv/bin/activate
```
Install numpy and opencv.
```
pip install numpy
pip install pyopencv
```

Now you can execute the histogram generation.
```
python3 histogram_gen.py -path_with_images "PathToImageFiles"
```
where "PathToImageFiles" is your local path to where the images you want "histogramizied" are located.
The input from this operation is in the folder "hist_gen_output" in the `python_scripts` folder.

## Deployment
### Publishing / building the application
To deploy the application, go to `./ImageFinderUserStudy/ImageFinderUserStudyWeb` and execute following command.
```
dotnet publish -c Release -r win-x64 --self-contained true --output [OutputPath] ImageFinderUserStudyWeb.csproj
```
This command is going to work both on Linux and Windows.

### Running the application at server
This previous command sets the application to run at IIS. The application is already configured that way.
Copy the output from the publish and paste it into the desired location at the server.
Configure the IIS and add a new Site in IIS according to this guide: https://weblog.west-wind.com/posts/2016/jun/06/publishing-and-running-aspnet-core-applications-with-iis

Now, you need to copy Google Image Labels files and Color Histogram files to root folder. Go to the location where the application lives on the server.
Find the correct label files and histogram files and put them in `./Resources/GoogleLabelsFiles` and `./Resources/ColorHistogramFiles` respectively.
After that, go to wwwroot folder and paste the image files under `/./wwwroot/Resources/ImageFiles`.
Note two things
1) The path to the images, label files and color histograms can be set-up in appsettings.json. This description is for the default behavior.
2) The Resources folder in the wwwroot is not the same as the Resources folder in the root folder.

Enable directory browsing so Image Finder can load images from the disk https://docs.microsoft.com/en-us/iis/configuration/system.webserver/directorybrowse.

## Changing the application parameters
### appsettings.json
In the root directory of the application, in appsettings.json, there are parameters for the application. E.g. gallery width, probabilities, etc.
To change a parametes, change it in the appsettings.json and restart the application.

## Changing application behavior
### Image sorting
Every possible sorter has its own service.
In this service, there can be multiple functions
```
private string[,] SortBySomething(var args) { ... }
```
To add another sorter, it is suggested to add an enum to the respective service for the sorting type, declare this type in appsettings.json and use switch case to decide which sorter to use according to the settings. This patter can be found with "GalleryType" setting in appsettings.json. Follow this pattern to implement a different sorter.
To be more object oriented, a sorter class could be added for a given service responsible for different types of sorting.

For example, I could want two types of sorters: Random and Cascade (it doesn't matter what cascade means in this case, it's just an example).
In LabelsSorterService, I can create enum
```
public enum LabelSortersType
{
  Random,
  Cascade
}
```
This enum, I can add to appsettings.json as
```
"LabelSorterType": 1
```
Which can be translated to `GlobalConfig` in `StartUp` as
```
int.TryParse(configuration["GallerySettings:LabelSorterType"], out var labelSorterType);
```
Then, when function
```
public SorterOutput Sort (var args) { ... }
```
is called, I can do a check inside this function.
```
if (GlobalConfig.LabelSorterType == LabelSorterType.Random)
{
  sorterOutput = SortByRandom(args);
}
else if (GlobalConfig.LabelSorterType == LabelSorterType.Cascade)
{
  sorterOutput = SortByCascade(args);
}
```
