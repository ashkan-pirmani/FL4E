import warnings
import flwr as fl
import numpy as np

from sklearn.linear_model import LogisticRegression
from sklearn.metrics import log_loss

import utils
import argparse


def main(agent_id, server_address):
    # Create LogisticRegression Model
    model = LogisticRegression(
        penalty="l2",
        max_iter=1, # local epoch
        warm_start=True, # prevent refreshing weights when fitting
        C = 0.1,
    )

    utils.set_initial_params(model)

    fl.client.start_numpy_client(server_address, client = FederatedClient(model,agent_id))

class FederatedClient(fl.client.NumPyClient):
    def __init__(self,model,agent_id):
        super().__init__()
        self.agent_id = agent_id
        self.model = model
        self.X, self.y = utils.load_data(agent_id)
    def get_parameters(self): # type: ignore
        return utils.get_model_parameters(self.model)

    def fit(self, parameters, config): # type: ignore
        utils.set_model_params(self.model, parameters)
        # Ignore convergence failure due to low local epochs
        with warnings.catch_warnings():
            warnings.simplefilter("ignore")
            self.model.fit(self.X, self.y)
            print(f"Training finished for round {config['rnd']}")
        return utils.get_model_parameters(self.model), len(self.X), {}

    def evaluate(self, parameters, config): # type: ignore
        utils.set_model_params(self.model, parameters)
        loss = log_loss(y_test, self.model.predict_proba(X_test))
        accuracy = self.model.score(X_test, y_test)
        return loss, len(X_test), {"accuracy": accuracy}

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description='Client side.')
    parser.add_argument('--agent_id',  type=str, help = "ID of the client, only for testing purposes")
    parser.add_argument('--server_address',  type=str, help = "ID of the client, only for testing purposes", default = "localhost:8889")

    args = parser.parse_args()
    print(args)
    main(**vars(args))
