# PolitiCriticAPI
HackGT4 PolitiCritic API

A website that uses a Naive Bayes classifier to estimate political leaning of social media accounts. Link your facebook page or enter your reddit username to get our estimation on your political leaning! Estimates range from -1.0 to 1.0, with -1.0 being liberal, 1.0 being conservative, and 0.0 being moderate.

Backend API has two publically available endpoints:

http://politicritic.azurewebsites.net/leaning/facebook?user={Facebook_page_URL}

http://politicritic.azurewebsites.net/leaning/reddit?user={Reddit_username}
