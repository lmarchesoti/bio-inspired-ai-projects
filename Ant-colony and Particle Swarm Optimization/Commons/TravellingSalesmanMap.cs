using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Commons {
    public class TravellingSalesmanMap {
        //matriz de distancias do problema
        protected double[,] DistanceMatrix;
        //vetor que contem o nome de cada cidade
        private string[] CitiesAlias = null;
        //quantidade de cidades
        public int CityCount { get; private set; }
        //valor da solução ótima
        public double? OptimalTravelDistance { get; private set; }

        public MapInstance map;
        
        //instancias de mapa padrão suportadas pelo construtor
        public enum MapInstance {
            M06,
            M15,
            BAY29,
            M29,
            M38,
            ATT48,
            BRAZIL58,
            ST70,
            EIL76,
            RAT99
        };

        //tipo de arquivo para criação da matriz de distancias
        public enum DescriptionType {
            Coordinates,
            Distances
        }

        //cria uma instancia do problema a partir de um nome de mapa
        public TravellingSalesmanMap(MapInstance map) {
            this.map = map;
            switch (map) {
                case MapInstance.M06:
                    DistanceMatrix = ReadDistancesFile("..\\..\\..\\Commons\\MapDescriptions\\M06_Distances.txt");
                    //CitiesAlias = new string[] { "Aveiro", "Badajoz", "Cordoba", "Evora", "Faro", "Madrid" };
            	    break;
                case MapInstance.M15:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\M15_Coordinates.txt");
                    break;
                case MapInstance.BAY29:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\BAY29_Coordinates.txt");
                    break;
                case MapInstance.M29:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\M29_Coordinates.txt");
                    break;
                case MapInstance.M38:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\M38_Coordinates.txt");
                    break;
                case MapInstance.ATT48:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\ATT48_Coordinates.txt");
                    break;
                case MapInstance.BRAZIL58:
                    DistanceMatrix = ReadDistancesFile("..\\..\\..\\Commons\\MapDescriptions\\BRAZIL58_Distances.txt");
                    break;
                case MapInstance.ST70:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\ST70_Coordinates.txt");
                    break;
                case MapInstance.EIL76:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\EIL76_Coordinates.txt");
                    break;
                case MapInstance.RAT99:
                    DistanceMatrix = ReadCoordinatesFile("..\\..\\..\\Commons\\MapDescriptions\\RAT99_Coordinates.txt");
                    break;
            }
            CityCount = (int)Math.Sqrt(DistanceMatrix.Length);
        }

        //cria uma instancia do problema a partir do tipo e arquivo especificados
        public TravellingSalesmanMap(string filePath, DescriptionType type) {
            switch (type) {
                case DescriptionType.Coordinates:
                    DistanceMatrix = ReadCoordinatesFile(filePath);
                    break;
                case DescriptionType.Distances:
                    DistanceMatrix = ReadDistancesFile(filePath);
                    break;
            }
            CityCount =  (int)Math.Sqrt(DistanceMatrix.Length);
        }

        //retorna o nome da cidade a partir do numero
        public string GetCityAlias(int cityNumber) {
            if (CitiesAlias != null)
                return CitiesAlias[cityNumber - 1];
            else
                return Convert.ToString(cityNumber);
        }

        //retorna a distancia entre duas cidades
        public double GetDistanceBetween(int cityA, int cityB) {
            return DistanceMatrix[cityA - 1, cityB - 1];
        }

        //cria a matriz de distancias a partir do arquivo de coordenadas
        private double[,] ReadCoordinatesFile(string filePath) {
            double[,] distancesMatrix = null;
            using (StreamReader file = new StreamReader(filePath)) {
                string fileContent = file.ReadToEnd();

                string[] fileLines = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                string[] tempSplit = fileLines[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (tempSplit.Length > 0) {
                    string optimaValueStr = tempSplit[0];
                    OptimalTravelDistance = double.Parse(optimaValueStr);
                }
                else
                    OptimalTravelDistance = null;
                string[] coordinates = fileLines.Skip(1).ToArray();
                int amoutOfCities = coordinates.Length;
                double[,] coordinatesMatrix = new double[amoutOfCities, 2];
                foreach (string coordinate in coordinates) {
                    string[] terms = coordinate.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                    int cityNumber = int.Parse(terms[0]);
                    double cityXCoordinate = double.Parse(terms[1], CultureInfo.GetCultureInfo("En-US"));
                    double cityYCoordinate = double.Parse(terms[2], CultureInfo.GetCultureInfo("En-US"));
                    coordinatesMatrix[cityNumber - 1, 0] = cityXCoordinate;
                    coordinatesMatrix[cityNumber - 1, 1] = cityYCoordinate;
                }
                distancesMatrix = new double[amoutOfCities, amoutOfCities];

                for (int cityA = 1; cityA <= amoutOfCities; ++cityA) {
                    double cityAXCoordinate = coordinatesMatrix[cityA - 1, 0];
                    double cityAYCoordinate = coordinatesMatrix[cityA - 1, 1];
                    for (int cityB = cityA + 1; cityB <= amoutOfCities; ++cityB) {
                        double cityBXCoordinate = coordinatesMatrix[cityB - 1, 0];
                        double cityBYCoordinate = coordinatesMatrix[cityB - 1, 1];
                        double distanceBetweenCities = Math.Sqrt(Math.Pow(cityAXCoordinate - cityBXCoordinate, 2) + Math.Pow(cityAYCoordinate - cityBYCoordinate, 2));
                        distancesMatrix[cityA - 1, cityB - 1] = distanceBetweenCities;
                        distancesMatrix[cityB - 1, cityA - 1] = distanceBetweenCities;
                    }
                    distancesMatrix[cityA - 1, cityA - 1] = 0;
                }
            }
            return distancesMatrix;
        }

        //lê a matriz de distancias do arquivo
        private double[,] ReadDistancesFile(string filePath) {
            double[,] distancesMatrix = null;
            using (StreamReader file = new StreamReader(filePath)) {
                string fileContent = file.ReadToEnd();

                string[] fileLines = fileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                string optimaValueStr = fileLines[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                OptimalTravelDistance = double.Parse(optimaValueStr);
                string[] matrixLines = fileLines.Skip(1).ToArray();
                int amoutOfCities = matrixLines.Length + 1;
                double[,] coordinatesMatrix = new double[amoutOfCities, 2];
                distancesMatrix = new double[amoutOfCities, amoutOfCities];
                for (int cityA = 1; cityA < amoutOfCities; ++cityA) {
                    string[] distancesFromA = matrixLines[cityA - 1].Split(' ');
                    for (int cityB = cityA + 1; cityB <= amoutOfCities; ++cityB) {
                        double distanceFromAToB = double.Parse(distancesFromA[cityB - 1 - cityA], CultureInfo.GetCultureInfo("En-US"));
                        distancesMatrix[cityA - 1, cityB - 1] = distanceFromAToB;
                        distancesMatrix[cityB - 1, cityA - 1] = distanceFromAToB;
                    }
                    distancesMatrix[cityA - 1, cityA - 1] = 1;
                }
                distancesMatrix[amoutOfCities - 1, amoutOfCities - 1] = 0;
            }
            return distancesMatrix;
        }
    }
}
