# Image Finder User Study
Simple web application for user study, letting users find a presented image in a gallery

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

### Running the application
This previous command sets the application to run at IIS. The application is already configured that way.
Copy the output from the publish and paste it into the desired location at the server.
Configure the IIS and add a new Site in IIS according to this guide: https://weblog.west-wind.com/posts/2016/jun/06/publishing-and-running-aspnet-core-applications-with-iis

Now, you need to copy Google Image Labels files and Color Histogram files to root folder. Go to the location where the application lives on the server.
Find the correct label files and histogram files and put them in `./Resources/GoogleLabelsFiles` and `./Resources/ColorHistogramFiles` respectively.
After that, go to wwwroot folder and paste the image files under `/./wwwroot/Resources/ImageFiles`.
Note two things
1) The path to the images, label files and color histograms can be set-up in appsettings.json. This description is for the default behavior.
2) The Resources folder in the wwwroot is not the same as the Resources folder in the root folder.
