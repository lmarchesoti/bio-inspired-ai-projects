#pragma once
#include <vector>
using namespace std;

class Individuo
{
protected:
	double fitness;
public:

	virtual void fav() = 0;

	virtual void mutate() = 0;

	virtual bool better(Individuo*) = 0;
	
	virtual vector<Individuo*> cross(void*) = 0;

	virtual bool ehSolucao() = 0;

	virtual void print() = 0;

	double getFitness();
	
};

bool compare(Individuo*, Individuo*);
