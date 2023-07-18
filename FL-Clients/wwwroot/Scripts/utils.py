from typing import Tuple, Union, List
import numpy as np
from sklearn.linear_model import LogisticRegression
import pandas as pd

# Information : https://flower.dev/blog/2021-07-21-federated-scikit-learn-using-flower

XY = Tuple[np.ndarray, np.ndarray]
Dataset = Tuple[XY, XY]
LogRegParams = Union[XY, Tuple[np.ndarray]]
XYList = List[XY]


def get_model_parameters(model):
    """Returns the paramters of a sklearn LogisticRegression model"""
    if model.fit_intercept:
        params = (model.coef_, model.intercept_)
    else:
        params = (model.coef_,)
    return params

def set_model_params(
    model: LogisticRegression, params: LogRegParams
) -> LogisticRegression:
    """Sets the parameters of a sklean LogisticRegression model"""
    model.coef_ = params[0]
    if model.fit_intercept:
        model.intercept_ = params[1]
    return model

def set_initial_params(model: LogisticRegression):
    """
    Sets initial parameters as zeros
    """
    n_classes = 2 # HeartFailure data has 2 classes
    n_features = 10 # Number of features in dataset
    model.classes_ = np.array([i for i in range(n_classes)])

    model.coef_ = np.zeros((n_classes, n_features))
    if model.fit_intercept:
        model.intercept_ = np.zeros((n_classes,))


def load_test_data() -> Dataset:
    """
    Loads the dataset
    """
    df = pd.read_csv("/app/wwwroot/Scripts/central_test.csv") # the test data is on the central server

    var_names = ['mean radius', 'mean texture', 'mean perimeter', 'mean area',
       'mean smoothness', 'mean compactness', 'mean concavity',
       'mean concave points', 'mean symmetry', 'mean fractal dimension']

    pred_name = ["target"]
    x_test = df[var_names].values
    y_test = df[pred_name].values

    return (x_test, y_test)

def load_data(agent_id = 0) -> Dataset:
    """
    Loads the dataset
    """
    df = pd.read_csv(f"/app/wwwroot/Scripts/federated_{agent_id}.csv")

    var_names = ['mean radius', 'mean texture', 'mean perimeter', 'mean area',
   'mean smoothness', 'mean compactness', 'mean concavity',
   'mean concave points', 'mean symmetry', 'mean fractal dimension']

    pred_name = ["target"]
    x_train = df[var_names].values
    y_train = df[pred_name].values.ravel()

    return (x_train, y_train)

def partition(X: np.ndarray, y: np.ndarray, num_partitions: int) -> XYList:
    """Split X and y into a number of partitions."""
    return list(
        zip(np.array_split(X, num_partitions),
        np.array_split(y, num_partitions))
    )
