#include "individuo.h"

double Individuo::getFitness(){
	return this->fitness;
}

bool compare(Individuo *individuo1, Individuo *individuo2){
	return individuo1->better(individuo2);
}
