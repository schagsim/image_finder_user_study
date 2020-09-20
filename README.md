# Image Finder User Study
Simple web application for user study, letting users find a presented image in a gallery

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
