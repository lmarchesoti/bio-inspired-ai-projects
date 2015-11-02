#include "individuo_impl.h"
#include <iostream>
#include <time.h>
#include <algorithm>
#include <string.h>
using namespace std;

#ifndef PREREQ_
#define PREREQ_
static vector<vector<int> > MakeVector()
{
    vector<vector<int> > v;
	vector<int> vcontent;
	//00
	vcontent.clear();
	v.insert(v.begin() + 0, vcontent);
	//01, 02, 03, 04, 05
	vcontent.clear();
	vcontent.push_back(0);
	v.insert(v.begin() + 1, vcontent);
	v.insert(v.begin() + 2, vcontent);
	v.insert(v.begin() + 3, vcontent);
	v.insert(v.begin() + 4, vcontent);
	v.insert(v.begin() + 5, vcontent);
	//06
	vcontent.clear();
//	vcontent.push_back(0);
	vcontent.push_back(2);
	v.insert(v.begin() + 6, vcontent);
	//07
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
	vcontent.push_back(6);
	v.insert(v.begin() + 7, vcontent);
	//08
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
	vcontent.push_back(3);
	vcontent.push_back(6);
	v.insert(v.begin() + 8, vcontent);
	//09
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
	vcontent.push_back(4);
	vcontent.push_back(6);
	v.insert(v.begin() + 9, vcontent);
	//10
	vcontent.clear();
	vcontent.push_back(0);
	vcontent.push_back(2);
	vcontent.push_back(5);
	vcontent.push_back(6);
	v.insert(v.begin() + 10, vcontent);
	//11
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
	vcontent.push_back(6);
	vcontent.push_back(8);
	v.insert(v.begin() + 11, vcontent);
	//12
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(6);
	vcontent.push_back(8);
	vcontent.push_back(11);
	v.insert(v.begin() + 12, vcontent);
	//13
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(4);
//	vcontent.push_back(6);
//	vcontent.push_back(8);
	vcontent.push_back(9);
	vcontent.push_back(11);
	v.insert(v.begin() + 13, vcontent);
	//14
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(5);
//	vcontent.push_back(6);
//	vcontent.push_back(8);
	vcontent.push_back(10);
	vcontent.push_back(11);
	v.insert(v.begin() + 14, vcontent);
	//15
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(4);
//	vcontent.push_back(6);
//	vcontent.push_back(8);
//	vcontent.push_back(9);
	vcontent.push_back(11);
	vcontent.push_back(13);
	v.insert(v.begin() + 15, vcontent);
	//16
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(4);
//	vcontent.push_back(6);
//	vcontent.push_back(8);
//	vcontent.push_back(9);
//	vcontent.push_back(11);
	vcontent.push_back(13);
	vcontent.push_back(15);
	v.insert(v.begin() + 16, vcontent);
	//17
	vcontent.clear();
//	vcontent.push_back(0);
//	vcontent.push_back(2);
//	vcontent.push_back(3);
//	vcontent.push_back(4);
//	vcontent.push_back(5);
//	vcontent.push_back(6);
//	vcontent.push_back(8);
//	vcontent.push_back(9);
//	vcontent.push_back(10);
//	vcontent.push_back(11);
//	vcontent.push_back(13);
	vcontent.push_back(14);
	vcontent.push_back(15);
	v.insert(v.begin() + 17, vcontent);
    return v;
}


static const vector<vector<int> > prereq = MakeVector();
#endif

#ifndef CAMINHO_
#define CAMINHO_
static vector<vector<int> > MakeCaminho()
{
    vector<vector<int> > v;
	vector<int> vcontent;
	//00
	vcontent.clear();
	vcontent.push_back(1);
	vcontent.push_back(2);
	vcontent.push_back(3);
	vcontent.push_back(4);
	vcontent.push_back(5);
	vcontent.push_back(6);
	v.insert(v.begin() + 0, vcontent);
	//01
	vcontent.clear();
	v.insert(v.begin() + 1, vcontent);
	//02
	vcontent.clear();
	vcontent.push_back(6);
	vcontent.push_back(7);
	v.insert(v.begin() + 2, vcontent);
	//03
	vcontent.clear();
	vcontent.push_back(8);
	v.insert(v.begin() + 3, vcontent);
	//04
	vcontent.clear();
	vcontent.push_back(9);
	v.insert(v.begin() + 4, vcontent);
	//05
	vcontent.clear();
	vcontent.push_back(10);
	v.insert(v.begin() + 5, vcontent);
	//06
	vcontent.clear();
	vcontent.push_back(7);
	vcontent.push_back(8);
	vcontent.push_back(9);
	vcontent.push_back(10);
	vcontent.push_back(11);
	v.insert(v.begin() + 6, vcontent);
	//07
	vcontent.clear();
	v.insert(v.begin() + 7, vcontent);
	//08
	vcontent.clear();
	vcontent.push_back(11);
	vcontent.push_back(12);
	v.insert(v.begin() + 8, vcontent);
	//09
	vcontent.clear();
	vcontent.push_back(13);
	v.insert(v.begin() + 9, vcontent);
	//10
	vcontent.clear();
	vcontent.push_back(14);
	v.insert(v.begin() + 10, vcontent);
	//11
	vcontent.clear();
	vcontent.push_back(12);
	vcontent.push_back(13);
	vcontent.push_back(14);
	vcontent.push_back(15);
	v.insert(v.begin() + 11, vcontent);
	//12
	vcontent.clear();
	v.insert(v.begin() + 12, vcontent);
	//13
	vcontent.clear();
	vcontent.push_back(15);
	vcontent.push_back(16);
	v.insert(v.begin() + 13, vcontent);
	//14
	vcontent.clear();
	vcontent.push_back(17);
	v.insert(v.begin() + 14, vcontent);
	//15
	vcontent.clear();
	vcontent.push_back(16);
	vcontent.push_back(17);
	v.insert(v.begin() + 15, vcontent);
	//16, 17
	vcontent.clear();
	v.insert(v.begin() + 16, vcontent);
	v.insert(v.begin() + 17, vcontent);
    return v;
}


static const vector<vector<int> > caminho = MakeCaminho();
#endif

#ifndef CUSTOS_
#define CUSTOS_
	static const int custoTarefa[NTAREFAS] = {8, 4, 4, 4, 4, 4, 6, 3, 3, 3, 3, 4, 2, 2, 2, 2, 1, 1};
	double custo[NTAREFAS];
#endif

IndividuoImpl::IndividuoImpl(void)
{
	escalonamento.reserve(NTAREFAS);
	int ntarefas[NTAREFAS], aleat;
	IndividuoStruct tarefaproc;
	for(int i = 0; i < NTAREFAS; ++i)
		ntarefas[i] = 0;
	for(int i = 0; i < NTAREFAS; ++i){
		while(ntarefas[(aleat = rand()%NTAREFAS)]);
		ntarefas[aleat] = 1;
		posicoes[aleat] = i;
		tarefaproc.tarefa = aleat;
		tarefaproc.processador = rand()%NPROC;
		escalonamento.push_back(tarefaproc);
	}
	reparar();
	fav();
}

IndividuoImpl::IndividuoImpl(const IndividuoImpl &individuo)
{
	this->escalonamento = individuo.escalonamento;
	memcpy(this->posicoes, individuo.posicoes, sizeof(individuo.posicoes));
	fav();
}

IndividuoImpl::~IndividuoImpl(void)
{
}
/*
// maior caminho. ignora o numero de processadores.
double IndividuoImpl::custoCaminho(int tarefa){
	double temp;
	int tempTarefa;
	//visita cada vertice no caminho recursivamente, retornando o custo de se percorrer por aquela posicao
	for(unsigned int i = 0; i < caminho.at(tarefa).size(); ++i){
		tempTarefa = caminho.at(tarefa).at(i);
		temp = custoCaminho(tempTarefa);
		//adiciona o valor de comunicacao caso necessario
		if(escalonamento.at(posicoes[tarefa]).processador != escalonamento.at(posicoes[tempTarefa]).processador)
			temp += COMCOST;
		temp += custoTarefa[tarefa];//adiciona o valor de processamento da tarefa
		if(temp > custo[tarefa])//substitui o valor do custo caso seja maior
			custo[tarefa] = temp;
	}
	return custo[tarefa];
}

void IndividuoImpl::fav(){
	for(int i = 0; i < NTAREFAS; ++i)
		custo[i] = custoTarefa[i];
	this->fitness = custoCaminho(0);
}
/**/

void IndividuoImpl::fav(){
	int tempo[NPROC], exec[NTAREFAS], tarefa, temporeq;
	for(int i = 0; i < NPROC; ++ i)
		tempo[i] = 0;
	for(int i = 0; i < NTAREFAS; ++ i)
		exec[i] = 0;
	for(int i = 0; i < NTAREFAS; ++i){
		tarefa = escalonamento.at(i).tarefa;
		for(int j = 0; j < prereq.at(tarefa).size(); ++j){
			temporeq = exec[prereq.at(tarefa).at(j)];
			if(escalonamento.at(i).processador != escalonamento.at(posicoes[prereq.at(tarefa).at(j)]).processador)
				temporeq += COMCOST;
			if(tempo[escalonamento.at(i).processador] < temporeq)
				tempo[escalonamento.at(i).processador] = temporeq;
		}
		tempo[escalonamento.at(i).processador] += custoTarefa[escalonamento.at(i).tarefa];
		exec[escalonamento.at(i).tarefa] = tempo[escalonamento.at(i).processador];
	}
	temporeq = 0;
	for(int i = 0; i < NPROC; ++i)
		temporeq = tempo[i] > temporeq ? tempo[i] : temporeq;
	this->fitness = temporeq;
}

void IndividuoImpl::mutate(){
	troca(rand()%NTAREFAS,rand()%NTAREFAS);
	reparar();
	fav();
}

bool IndividuoImpl::better(Individuo *individuo){
	return (this->getFitness() < individuo->getFitness());
}

bool IndividuoImpl::ehSolucao(){
	return false;
}

void IndividuoImpl::print(){
	cout << "Individuo | Fitness: " << this->getFitness() << endl;
	cout << "Tarefas:" << endl;
	for(vector<IndividuoStruct>::iterator it = escalonamento.begin(); it != escalonamento.end(); ++it){
		cout << "\t" << it->tarefa;
	}
	cout << endl;
	cout << "Processadores:" << endl;
	for(vector<IndividuoStruct>::iterator it = escalonamento.begin(); it != escalonamento.end(); ++it){
		cout << "\t" << it->processador;
	}
	cout << endl;
}

vector<Individuo*> IndividuoImpl::cross(void *ind){
	IndividuoImpl *individuo = (IndividuoImpl*) ind;
	IndividuoImpl *pai = new IndividuoImpl(*this);
	IndividuoImpl *mae = new IndividuoImpl(*individuo);
	int partida = rand()%NTAREFAS, temp = partida, troca[NTAREFAS];
	IndividuoStruct tempstruct;
	for(int i = 0; i < NTAREFAS; ++i)
		troca[i] = 0;
	while(pai->escalonamento.at(partida).tarefa != mae->escalonamento.at(temp).tarefa){
		troca[temp] = 1;
		temp = pai->posicoes[mae->escalonamento.at(temp).tarefa];
	}
	troca[temp] = 1;
	for(int i = 0; i < NTAREFAS; ++i)
		if(troca[i]){
			tempstruct = mae->escalonamento.at(i);
			mae->escalonamento.at(i) = pai->escalonamento.at(i);
			pai->escalonamento.at(i) = tempstruct;
		}
	for(int i = 0; i < NTAREFAS; ++i){
		pai->posicoes[pai->escalonamento.at(i).tarefa] = i;
		mae->posicoes[mae->escalonamento.at(i).tarefa] = i;
	}
	pai->reparar();
	mae->reparar();
	pai->fav();
	mae->fav();
	vector<Individuo*> individuos;
	individuos.push_back(pai);
	individuos.push_back(mae);
	return individuos;
}

void IndividuoImpl::reparar(){
	if(posicoes[0]) troca(0, escalonamento[0].tarefa);
	for(int i = 0; i < NTAREFAS; ++i)//para cada tarefa
		denovo:
		for(unsigned int j = 0; j < prereq.at(escalonamento.at(i).tarefa).size(); ++j)//para cada prioridade da lista de prioridades daquela tarefa
			if(posicoes[escalonamento.at(i).tarefa] < posicoes[prereq.at(escalonamento.at(i).tarefa).at(j)]){//se encontrar alguma das prioridades numa posicao maior que deveria
				troca(escalonamento.at(i).tarefa, prereq.at(escalonamento.at(i).tarefa).at(j));//troca as tarefas
				goto denovo;//reavalia a posicao
			}
}

void IndividuoImpl::troca(int x, int y){
	swap(escalonamento[posicoes[x]], escalonamento[posicoes[y]]);
	swap(posicoes[x], posicoes[y]);
}
