var AdminHomePageModule = (function (module) {

    module.Initialize = function(locations,visits) {
        new Chart(document.querySelector('canvas.visits-chart'), {
            type: 'doughnut',
            data: {
                labels: locations,
                datasets: [
                    {
                        label: "Most visits",
                        backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                        data: visits
                    }
                ]
            },
            options: {
                title: {
                    display: true,
                    text: 'Most site visits by country',
                    fontSize: 30
                }
            }
        });
    }

    return module;
}({}))
