#include "ag.h"
#include "parameters.h"
#include <iostream>
using namespace std;

AG::AG(void)
{
	while(populacao.size() < POPSIZE)
		this->populacao.push_back(new IndividuoImpl());
}

AG::~AG(void)
{
}

void AG::crossOver(){
	vector<Individuo*> filhos;
	evolutor.prepareCross();
	for(int i = 0; i < CROSSRATE * POPSIZE / 100; ++i){
		filhos = evolutor.selecionaCrossOver(populacao)->cross(evolutor.selecionaCrossOver(populacao));
		while(!filhos.empty()){
			populacao.push_back(filhos.back());
			filhos.pop_back();
		}
	}
}

void AG::mutacao(){
	for(int i = 0; i < MUTRATE * POPSIZE / 100; ++i){
		Individuo *individuo = this->evolutor.selecionaAleatorio(this->populacao);
		individuo->mutate();
	}
}

void AG::normalizacao(){
	this->populacao = ambiente.selecionar(this->populacao);
}

void AG::resultado(){
	#ifdef BATCH_
	cout << populacao.front()->getFitness();
	#else
	#ifdef SOLUCAO_
	if(populacao.front()->ehSolucao())
		cout << "Solucao encontrada." << endl;
	else
		cout << "Solucao NAO encontrada." << endl;
	#endif
	cout << "Imprimindo melhor individuo:" << endl;
	populacao.front()->print();
	cout << endl;
	#endif
}

void AG::rodarGeracao(){
	crossOver();
	mutacao();
	normalizacao();
}

void AG::executaAG(){
	for(int i = 0; i < NUMGER; i++){
		rodarGeracao();
	}
	resultado();
}
