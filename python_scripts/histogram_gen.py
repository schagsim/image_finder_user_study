#!/usr/bin/env python3

import argparse
import os

import glob
import cv2 as cv
import numpy
import json

from typing import List

SUPPORTED_EXTENSIONS = ["jpg", "png"]

# Add param from where to load the images.
# This script expects the images to be in format {unique_id}.(jpg|png)
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

# We get all the image files in the provided path.
google_image_files = []
for extension in SUPPORTED_EXTENSIONS:
    google_image_files.extend(
        glob.glob(
            pathname=f"{path}*.{extension}",
            recursive=False,
        )
    )

def _normalize_histogram(
    histogram,
) -> numpy.ndarray:
    """
    Normalize histogram values to the number of pixels.
    """
    return numpy.divide(
        histogram.astype(numpy.float32),
        numpy.sum(histogram).astype(numpy.float32)
    ).astype(numpy.float32)


def _extract_values_from_histogram(
    histogram,
) -> List[float]:
    extracted_values = []
    for hist_value in histogram:
        extracted_values.append(float(hist_value[0]))

    return extracted_values

def _get_id_from_file_path(
    file_path: str,
) -> str:
    parts = file_path.split('/')
    id_with_extension = parts[len(parts) - 1]
    id_split_by_extension = id_with_extension.split('.')
    id = id_split_by_extension[0]
    return id

# Create the output directory.
output_directory = "./hist_gen_output/"
if not os.path.exists(output_directory):
    print(f"Creating output directory {output_directory}")
    os.mkdir(output_directory)

# For each found image, we count the histogram and save it to file.
for file_path in google_image_files:
    # Get the ID of the image.
    id = _get_id_from_file_path(file_path)

    # First, delete the existing file.
    output_file = f"{output_directory}{id}.json"
    if os.path.exists(
        path=output_file
    ):
        os.remove(
            path=output_file
        )

    # Then we open the stream to which we will write.
    file_stream = open(output_file, "w+")

    # Load the image.
    image_read = cv.imread(
        filename=file_path,
    )
    number_of_pixels = image_read.size

    # B hist.
    blue_hist = cv.calcHist(
        images=[image_read],
        channels=[0],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    blue_hist = _normalize_histogram(blue_hist)
    blue_normalized_values = _extract_values_from_histogram(blue_hist)

    # G hist.
    green_hist = cv.calcHist(
        images=[image_read],
        channels=[1],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    green_hist = _normalize_histogram(green_hist)
    green_normalized_values = _extract_values_from_histogram(green_hist)

    # R hist.
    red_hist = cv.calcHist(
        images=[image_read],
        channels=[2],
        mask=None,
        histSize=[16],
        ranges=[0,256],
    )
    red_hist = _normalize_histogram(red_hist)
    red_normalized_values = _extract_values_from_histogram(red_hist)

    output = {
        "ImageId": id,
        "BlueHistogram": blue_normalized_values,
        "GreenHistogram": green_normalized_values,
        "RedHistogram": red_normalized_values,
    }
    json_output = json.dumps(output)
    file_stream.write(json_output)
    file_stream.close()
