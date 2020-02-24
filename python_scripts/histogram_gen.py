import argparse
import os

import glob
import cv2

parser = argparse.ArgumentParser()
parser.add_argument(
    '-path_with_images',
    action='store',
    default=os.getcwd(),
    type=str,
    dest='path_with_images',
    help="Folder with images from which histograms shall be generated.",
)

args = parser.parse_args()
path = args.path_with_images

supported_extensions = ["jpg", "png"]

# We get all the image files in the provided path.
google_image_files = []
for extension in supported_extensions:
    google_image_files.extend(
        glob.glob(
            pathname=f"{path}*.{extension}",
            recursive=False,
        )
    )

# For each found image, we count the histogram and save it to file.
# First, delete the existing file.
output_file = "color_histogram.txt"
if os.path.exists(
    path=output_file
):
    os.remove(
        path=output_file
    )

# Then we open the stream to which we will write.
file_stream = open(output_file, "w+")

for file_path in google_image_files:
    image_read = cv2.imread(
        filename=file_path,
    )
    blue_hist = cv2.calcHist(
        images=[image_read],
        channels=[0],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    green_hist = cv2.calcHist(
        images=[image_read],
        channels=[1],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    red_hist = cv2.calcHist(
        images=[image_read],
        channels=[2],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    file_stream.write(f"{file_path}\n")
    file_stream.write(f"{blue_hist}\n")
    file_stream.write(f"{green_hist}\n")
    file_stream.write(f"{red_hist}\n")
