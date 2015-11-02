#include "evolutor.h"
#include <time.h>

Evolutor::Evolutor(void)
{
	srand(time(NULL));
}


Evolutor::~Evolutor(void)
{
}


Individuo* Evolutor::selecionaAleatorio(vector<Individuo*> populacao){
	return populacao.at(rand()%populacao.size());
}

#ifdef TORNEIO_

void Evolutor::prepareCross(){
	return;
}

Individuo* Evolutor::selecionaCrossOver(vector<Individuo*> populacao){
	
	Individuo *individuotemp, *individuo = selecionaAleatorio(populacao);

	for(int i = 1; i < TOUR; i++){
		individuotemp = selecionaAleatorio(populacao);
		individuo = individuo->better(individuotemp) ? individuo : individuotemp;
	}

	return individuo;

}

#endif
