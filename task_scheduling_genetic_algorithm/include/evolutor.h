#include "individuo.h"
#include "parameters.h"
#include <vector>
#include <cstdlib>
using namespace std;

class Evolutor
{
public:

	Evolutor(void);
	~Evolutor(void);

	Individuo* selecionaAleatorio(vector<Individuo*>);

	Individuo* selecionaCrossOver(vector<Individuo*>);

	void prepareCross();

};

