import argparse
import os

os.environ["CUDA_VISIBLE_DEVICES"]="-1"  

import tensorflow as tf

IMG_SIZE = 160 # All images will be resized to 160x160
IMG_SHAPE = (IMG_SIZE, IMG_SIZE, 3)

BATCH_SIZE = 10

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

print(path)

image_generator = tf.keras.preprocessing.image.ImageDataGenerator(rescale=1./255)

train_data_gen = image_generator.flow_from_directory(
    directory=path,
    batch_size=BATCH_SIZE,
    shuffle=True,
    target_size=(IMG_SIZE, IMG_SIZE),
    class_mode='binary'
)
image_batch, label_batch = next(train_data_gen)
print(f"Batch shape: {image_batch.shape}")

# Create the base model from the pre-trained model MobileNet V2
base_model = tf.keras.applications.MobileNetV2(
    input_shape=IMG_SHAPE,
    include_top=False,
    weights='imagenet'
)
feature_batch = base_model(image_batch)
print(f"Feature batch shape: {feature_batch.shape}")

# Turn off the base model training capabilities.
base_model.trainable = False