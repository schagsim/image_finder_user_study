import argparse
import os

import glob
import cv2
import numpy

from typing import List

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

def normalize_histogram(
    histogram,
) -> numpy.ndarray:
    return numpy.divide(
        histogram.astype(numpy.float32),
        numpy.sum(histogram).astype(numpy.float32)
    ).astype(numpy.float32)
    """
    hist_normalized = []
    for range_value in histogram[0]:
        hist_normalized.append(
            range_value[0] / number_of_pixels
        )
    return hist_normalized
    """

def extract_values_from_histogram(histogram) -> List[numpy.float32]:
    extracted_values = []
    for hist_value in histogram:
        extracted_values.append(hist_value[0])

    return extracted_values

# Then we open the stream to which we will write.
file_stream = open(output_file, "w+")

for file_path in google_image_files:
    image_read = cv2.imread(
        filename=file_path,
    )
    number_of_pixels = image_read.size
    print(number_of_pixels)
    blue_hist = cv2.calcHist(
        images=[image_read],
        channels=[0],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    blue_hist = normalize_histogram(blue_hist)
    blue_normalized_values = extract_values_from_histogram(blue_hist)

    green_hist = cv2.calcHist(
        images=[image_read],
        channels=[1],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    green_hist = normalize_histogram(green_hist)
    green_normalized_values = extract_values_from_histogram(green_hist)

    red_hist = cv2.calcHist(
        images=[image_read],
        channels=[2],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    red_hist = normalize_histogram(red_hist)
    red_normalized_values = extract_values_from_histogram(red_hist)

    file_stream.write(f"{file_path}\n")
    file_stream.write(f"{blue_normalized_values}\n")
    file_stream.write(f"{green_normalized_values}\n")
    file_stream.write(f"{red_normalized_values}\n")
