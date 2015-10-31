using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genetic_Algorithm_Commons.Util {
    public class CommunicationGraph {
        HashSet<int>[] allEdgeDependencies;
        List<int>[] directEdgeDependencies;
        List<int>[] edgeExpansions;
        int[] edgeCost;
        int edgeCount;
        int[][] dependencyMatrix;

        public CommunicationGraph(int[][] dependencyMatrix) {
            this.dependencyMatrix = dependencyMatrix;
            edgeCount = dependencyMatrix.Length;
            edgeCost = new int[edgeCount];
            allEdgeDependencies = new HashSet<int>[edgeCount];
            directEdgeDependencies = new List<int>[edgeCount];
            edgeExpansions = new List<int>[edgeCount];

            for (int edge = 0; edge < edgeCount; ++edge) {
                allEdgeDependencies[edge] = new HashSet<int>();
                directEdgeDependencies[edge] = new List<int>();
                edgeExpansions[edge] = new List<int>();
                for (int lineIdx = 0; lineIdx < edgeCount; ++lineIdx) {
                    if (lineIdx != edge) {
                        if (dependencyMatrix[lineIdx][edge] != -1) {
                            int dependency = lineIdx;
                            directEdgeDependencies[edge].Add(dependency);
                            allEdgeDependencies[edge].Add(dependency);
                            foreach (int subDependency in allEdgeDependencies[dependency]) {
                                allEdgeDependencies[edge].Add(subDependency);
                            }
                        }
                    }
                }
                for (int columnIdx = 0; columnIdx < edgeCount; ++columnIdx) {
                    if (columnIdx != edge) {
                        if (dependencyMatrix[edge][columnIdx] != -1) {
                            int expansion = columnIdx;
                            edgeExpansions[edge].Add(expansion);
                        }
                    }
                }
                edgeCost[edge] = dependencyMatrix[edge][edge];
            }
        }

        public bool DependsOn(int edgeA, int edgeB) {
            return allEdgeDependencies[edgeA].Contains(edgeB);
        }

        public int GetEdgeCost(int edge) {
            return edgeCost[edge];
        }

        public int GetCommunicationCost(int edgeA, int edgeB) {
            return dependencyMatrix[edgeA][edgeB];
        }

        public IEnumerable<int> GetAllDependencies(int edge) {
            return allEdgeDependencies[edge];
        }

        public IEnumerable<int> GetDirectDependencies(int edge) {
            return directEdgeDependencies[edge];
        }

        public IEnumerable<int> GetExpansions(int edge) {
            return edgeExpansions[edge];
        }
        //public 
    }
    /*
    #ifndef SEARCH_TREE_H
#define SEARCH_TREE_H

#include <deque>
#include <vector>
#include <queue>
#include <iostream>
#include <fstream>
#include <string>
#include <set>

class SearchTree {
	class CommGraph {
		std::vector<std::vector<int>*> expansionList;
		std::vector<std::vector<int>*> dependencyList;
		int quantityOfVertices;
		std::vector<int> tasksCost;
		int _sumOfVerticesCost;
		int** Matrix;
	public:
		CommGraph( std::string Filepath );
		~CommGraph();
		void makeAdjacent( int Vertex_1, int Vertex_2, int Weigth );
		int getTaskCost( int Task );
		int getCommunicationCost( int Vertex_1, int Vertex_2 );
		int getQuantityOfVertices();
		int getSumOfVerticesCost();
		std::vector<int>* getVertexExpansionList( int Vertex );
		std::vector<int>* getVertexDependencyList( int Vertex );
	};
	class STNode {
	public:
		SearchTree* _mainTree;
		STNode* _fatherNode;
		int _task;
		int _processor ;
		int _comparisonValue;
		int _depth;
		long long _number;
		std::vector<int> _processorsStatus;
		std::deque<SearchTree::STNode*> _expNodeList;
		std::vector<int> _waitingTasks;
	public:
		STNode( SearchTree* Tree );
		STNode( int Task, int Processor, int Depth, SearchTree::STNode* FatherND, SearchTree* STree );
		~STNode();
		SearchTree* getMainTree();
		STNode* getFatherNode();
		int getTask();
		int getProcessor();
		int getComparisonValue();
		int getDepth();
		int getStatusForProcessor( int Processor );
		void setStatusForProcessor( int Value, int Processor );
		int estimateResult();
		STNode* getDependencyNode( int Task );
		std::vector<int>* getWaitingTasks();
		bool fulfilledTask( int Task );
		bool dependenciesFulfilledForTask( int DesiredTask );
		bool redundantExpansion( int ExpTask, int ExpProc );
		bool isIrrelevant();
		void depthExpansion();
		void breadthExpansion();
		void bestFirstExpansion();
		void calculateCosts();
		bool allTasksAllocated();
		void printInFile();
	};
	class AnswerType {
		std::vector<std::deque<int>*> _schedule;
		std::vector<int> _processorsStatus;
		int _timming;
	public:
		AnswerType( SearchTree::STNode *N );
		~AnswerType();
		void Print();
		int getValue();
	};
	class ComparisonContext {
	public:
		bool operator() (STNode*& STN1, STNode*& STN2) {
			if(STN1->getComparisonValue() >= STN2->getComparisonValue())
				return true;
			return false;
		}
	};
	int _numberOfProcessors;
	int _numberOfTasks;
	long long _numberOfNodes;
	CommGraph* _graph;
	STNode* _root;
	AnswerType* _answer;
	std::deque<STNode*> _expansionList;
	std::priority_queue<STNode*, std::deque<STNode*>, ComparisonContext> _ordExpansionList;
public:
	SearchTree( std::string DescriptorPath, int Processors );
	~SearchTree();
	inline int getNumberOfProcessors() { return _numberOfProcessors; };
	inline int getNumberOfTasks() { return _numberOfTasks; };
	CommGraph* getGraph() { return _graph; };
	AnswerType* getAnswer() { return _answer; };
	void addAllDependenciesForVertexIn( int Task, std::set<int>* VFD, CommGraph* G );
	std::priority_queue<STNode*, std::deque<STNode*>, ComparisonContext>* getOrderedExpansionList();
	void setAnswer( AnswerType* New_Answer );
	void depthExpansion();
	void breadthExpansion();
	void bestFirstExpansion();
	void pushFront( STNode* Element );
	void pushBack( STNode* Element );
	void pushOrdered( STNode* Element );
	void answerTreatment( STNode* Answer );
	void increaseNumberOfNodes();
	long long getNumberOfNodes();
};

SearchTree::CommGraph::CommGraph( std::string filePath ) {
	_sumOfVerticesCost = 0;
	std::ifstream inputFile(filePath);
	std::string line;
	if(std::getline(inputFile, line)) {
		unsigned int Position = 0;
		while(Position < line.find_last_of(',')) {
			int commaPos = line.find(',', Position);
			std::string tCostStr = line.substr(Position, commaPos);
			int tCost = std::atoi(tCostStr.c_str());
			_sumOfVerticesCost += tCost;
			tasksCost.push_back(tCost);
			Position = commaPos+1;
		}
		std::string tCostStr = line.substr(Position, line.size());
		int tCost = std::atoi(tCostStr.c_str());
		tasksCost.push_back(tCost);
		// Tasks Vector Initialized
	}
	quantityOfVertices = tasksCost.size();
	Matrix = new int* [quantityOfVertices];
	Matrix[0] = new int [quantityOfVertices*quantityOfVertices];
	for(int Row = 1; Row < quantityOfVertices; Row++)
		Matrix[Row] = Matrix[Row-1] + quantityOfVertices;
	for(int Row=0; Row<quantityOfVertices; Row++) {
		for(int Col=0; Col<quantityOfVertices; Col++)
			Matrix[Row][Col] = -1;
	}
	for(int Index=0; Index<quantityOfVertices; Index++) {
		expansionList.push_back(new std::vector<int>());
		dependencyList.push_back(new std::vector<int>());
	}
	while(std::getline(inputFile, line)) {
		int startPosition = 0;
		int endPosition = line.find(',');
		int Vertex_1 = std::atoi(line.substr(startPosition, endPosition).c_str());
		startPosition = endPosition+1;
		endPosition = line.find(',', startPosition);
		int Vertex_2 = std::atoi(line.substr(startPosition, endPosition).c_str());
		startPosition = endPosition+1;
		endPosition = line.find(';');
		int Weigth = std::atoi(line.substr(startPosition, endPosition).c_str());
		makeAdjacent(Vertex_1, Vertex_2, Weigth);
	}
}
SearchTree::CommGraph::~CommGraph() {};
void SearchTree::CommGraph::makeAdjacent( int Vertex_1, int Vertex_2, int Weigth ) {
	expansionList[Vertex_1]->push_back(Vertex_2);
	dependencyList[Vertex_2]->push_back(Vertex_1);
	Matrix[Vertex_1][Vertex_2] = Weigth;
	Matrix[Vertex_2][Vertex_1] = Weigth;
}
int SearchTree::CommGraph::getTaskCost( int Task ) {
	return tasksCost[Task];
}
int SearchTree::CommGraph::getCommunicationCost( int Vertex_1, int Vertex_2 ) {
	if((Vertex_1 >= 0) && (Vertex_2 >= 0))
		return Matrix[Vertex_1][Vertex_2];
	else
		return -1;
}
int SearchTree::CommGraph::getQuantityOfVertices() {
	return quantityOfVertices;
}
int SearchTree::CommGraph::getSumOfVerticesCost() {
	return _sumOfVerticesCost;
}
std::vector<int>* SearchTree::CommGraph::getVertexExpansionList( int Vertex ) {
	return expansionList[Vertex];
}
std::vector<int>* SearchTree::CommGraph::getVertexDependencyList( int Vertex ) {
	return dependencyList[Vertex];
}

SearchTree::STNode::STNode( SearchTree* STree ) {
	const int NumberOfProcs = STree->getNumberOfProcessors();
	const int NumberOfTasks = STree->getNumberOfTasks();
	_mainTree = STree;
	_fatherNode = NULL;
	_task = -1;
	_processor = 0;
	for(int Proc=0; Proc<_mainTree->getNumberOfProcessors(); ++Proc)
		_processorsStatus.push_back(0);
	_comparisonValue = 0;
	_depth = 0;
	for(int i=0; i<NumberOfTasks; ++i) {
		std::vector<int>* depList = STree->getGraph()->getVertexDependencyList(i);
		if(depList->empty())
			_waitingTasks.push_back(i);
	}
}
SearchTree::STNode::STNode( int Task, int Processor, int Depth, SearchTree::STNode* FatherND, SearchTree* STree ) {
	const int NumberOfProcs = STree->getNumberOfProcessors();
	const int NumberOfTasks = STree->getNumberOfTasks();
	_mainTree = STree;
	_task = Task;
	_processor = Processor;
	_depth = Depth;
	_fatherNode = FatherND;
	_number = _mainTree->getNumberOfNodes();
	std::vector<int>* fTasks = _fatherNode->getWaitingTasks();
	for (std::vector<int>::const_iterator itr = fTasks->begin(); itr != fTasks->end(); ++itr) {
		int expTask = *itr;
		if(expTask != _task)
			_waitingTasks.push_back(expTask);
	}
	std::vector<int>* vertexExpList = _mainTree->getGraph()->getVertexExpansionList(_task);
	for (std::vector<int>::const_iterator itr = vertexExpList->begin(); itr != vertexExpList->end(); ++itr) {
		int expTask = *itr;
		if(dependenciesFulfilledForTask(expTask))
			_waitingTasks.push_back(expTask);
	}
	calculateCosts();
}
SearchTree::STNode::~STNode() {
	if(_fatherNode != NULL) {
		_fatherNode->_expNodeList.pop_front();
		if(_fatherNode->_expNodeList.empty())
			delete _fatherNode;
	}
}
SearchTree* SearchTree::STNode::getMainTree() {
	return _mainTree;
}
SearchTree::STNode* SearchTree::STNode::getFatherNode() {
	return _fatherNode;
}
int SearchTree::STNode::getTask() {
	return _task;
}
int SearchTree::STNode::getProcessor() {
	return _processor;
}
int SearchTree::STNode::getComparisonValue() {
	return _comparisonValue;
}
int SearchTree::STNode::getDepth() {
	return _depth;
}
std::vector<int>* SearchTree::STNode::getWaitingTasks() {
	return &_waitingTasks;
}
SearchTree::STNode* SearchTree::STNode::getDependencyNode( int Task ) {
	SearchTree::STNode* nodePtr = _fatherNode;
	while(nodePtr->getTask() != Task)
		nodePtr = nodePtr->getFatherNode();
	return nodePtr;
}
int SearchTree::STNode::estimateResult() {
	SearchTree::STNode* nodePtr = this;
	int tmps, tprocs;
	CommGraph* Graph = _mainTree->getGraph();

	tmps = Graph->getSumOfVerticesCost();
	tprocs = 0;
	while (nodePtr->getFatherNode() != NULL) {
		tmps -= Graph->getTaskCost(nodePtr->getTask());
		nodePtr = nodePtr->getFatherNode();
	}
	for (int Proc = 0; Proc < _mainTree->getNumberOfProcessors(); ++Proc)
		tprocs += getStatusForProcessor(Proc);
	tmps += tprocs;
	tmps /= _mainTree->getNumberOfProcessors();
	if (tmps > _comparisonValue)
		return (tmps);
	return (_comparisonValue);
}
void SearchTree::STNode::calculateCosts() {
	CommGraph* Graph = getMainTree()->getGraph();
	int newValue = _fatherNode->getStatusForProcessor(_processor) + Graph->getTaskCost(_task);
	std::vector<int>* depList = Graph->getVertexDependencyList(_task);
	for (std::vector<int>::const_iterator itr = depList->begin(); itr != depList->end(); ++itr) {
		SearchTree::STNode* depNode = getDependencyNode(*itr);
		if(depNode->getProcessor() != _processor) {
			int depCost = depNode->getStatusForProcessor(depNode->getProcessor()) + Graph->getCommunicationCost(*itr, _task) + Graph->getTaskCost(_task);
			if(depCost > newValue)
				newValue = depCost;
		}
	}
	for(int i=0; i<_processor; ++i)
		_processorsStatus.push_back(_fatherNode->getStatusForProcessor(i));
	_processorsStatus.push_back(newValue);
	for(int i=_processor+1; i<_mainTree->getNumberOfProcessors(); ++i)
		_processorsStatus.push_back(_fatherNode->getStatusForProcessor(i));
	if(newValue > _fatherNode->getComparisonValue())
		_comparisonValue = newValue;
	else
		_comparisonValue = _fatherNode->getComparisonValue();
}
bool SearchTree::STNode::allTasksAllocated() {
	if(_depth == _mainTree->getNumberOfTasks())
		return true;
	return false;
}
int SearchTree::STNode::getStatusForProcessor( int Processor ) {
	return _processorsStatus[Processor];
}
bool SearchTree::STNode::fulfilledTask( int Task ) {
	SearchTree::STNode* ptrSTNode = this;
	while(ptrSTNode->getFatherNode() != NULL) {
		if(ptrSTNode->getTask() == Task)
			return true;
		ptrSTNode = ptrSTNode->getFatherNode();
	}
	return false;
}
bool SearchTree::STNode::dependenciesFulfilledForTask( int DesiredTask ) {
	std::vector<int>* depList = _mainTree->getGraph()->getVertexDependencyList(DesiredTask);
	for (std::vector<int>::const_iterator itr = depList->begin(); itr != depList->end(); ++itr) {
		if(!fulfilledTask(*itr))
			return false;
	}
	return true;
}
bool SearchTree::STNode::isIrrelevant() {
	if(_mainTree->getAnswer() != NULL) {
		if(_mainTree->getAnswer()->getValue() <= estimateResult())
			return true;
	}
	return false;
}
void SearchTree::STNode::depthExpansion() {
//	if(!isIrrelevant()) {
		//printInFile();
		if(allTasksAllocated()) {
			_mainTree->increaseNumberOfNodes();
			//_mainTree->answerTreatment(this);
			//AnswerType* A = new AnswerType(this);
			//A->Print();
			delete this;
		}
		else {
			const int NumberOfProcs = _mainTree->getNumberOfProcessors();
			for (std::vector<int>::const_iterator itr = _waitingTasks.begin(); itr != _waitingTasks.end(); ++itr) {
				for(int Proc=0; Proc<NumberOfProcs; Proc++) {
					//if(redundantExpansion(*itr, Proc))
					//	continue;
					//else
						_expNodeList.push_back(new SearchTree::STNode(*itr, Proc, _depth+1, this, _mainTree));
				}
			}
			for (std::deque<STNode*>::const_reverse_iterator itr=_expNodeList.rbegin(); itr != _expNodeList.rend(); ++itr)
				_mainTree->pushFront(*itr);
		}
//	}
//	else
//		delete this;
}
void SearchTree::STNode::breadthExpansion() {
	if(allTasksAllocated()) {
		_mainTree->answerTreatment(this);
		delete this;
	}
	else {
		if(isIrrelevant()) {
			delete this;
			return;
		}
		const int NumberOfProcs = _mainTree->getNumberOfProcessors();
		for (std::vector<int>::const_iterator itr = _waitingTasks.begin(); itr != _waitingTasks.end(); ++itr) {
			for(int Proc=0; Proc<NumberOfProcs; ++Proc) {
				if(redundantExpansion(*itr, Proc))
					continue;
				else
					_expNodeList.push_front(new SearchTree::STNode(*itr, Proc, _depth+1, this, _mainTree));
			}
		}
		for (std::deque<STNode*>::const_reverse_iterator itr=_expNodeList.rbegin(); itr != _expNodeList.rend(); ++itr)
			_mainTree->pushBack(*itr);
	}
}
void SearchTree::STNode::bestFirstExpansion() {
	if(allTasksAllocated()) {
		_mainTree->answerTreatment(this);
		std::priority_queue<STNode*, std::deque<STNode*>, ComparisonContext>* ExpList = _mainTree->getOrderedExpansionList();
		while(!ExpList->empty())
			ExpList->pop();
		delete this;
	}
	else {
		const int NumberOfProcs = _mainTree->getNumberOfProcessors();
		for (std::vector<int>::const_iterator itr = _waitingTasks.begin(); itr != _waitingTasks.end(); ++itr) {
			for(int Proc=0; Proc<NumberOfProcs; ++Proc) {
				if(redundantExpansion(*itr, Proc))
					continue;
				else {
					STNode* NewNode = new SearchTree::STNode(*itr, Proc, _depth+1, this, _mainTree);
					_expNodeList.push_front(NewNode);
					_mainTree->_ordExpansionList.push(NewNode);
				}
			}
		}
	}
}
bool SearchTree::STNode::redundantExpansion( int ExpTask, int ExpProc ) {
	if(_processor != ExpProc) {
		if(_mainTree->getGraph()->getCommunicationCost(_task, ExpTask) == -1) {
			if(_task < ExpTask)
				return true;
		}
	}
	return false;
}
void SearchTree::STNode::printInFile() {
	std::ofstream myfile ("NodesExpansion.txt", std::ios::out | std::ios::app);
	STNode* nodePtr = this;
	while(nodePtr->getFatherNode() != NULL) { 
		myfile << "( " << nodePtr->getTask() << " | " << nodePtr->getProcessor() << " ) -- ";
		nodePtr = nodePtr->getFatherNode();
	}
	myfile << "\n";
}

void SearchTree::addAllDependenciesForVertexIn( int Task, std::set<int>* VFD, SearchTree::CommGraph* G ) {
	std::vector<int>* depList = G->getVertexDependencyList(Task);
	for(std::vector<int>::iterator itr=depList->begin(); itr != depList->end(); ++itr) {
		VFD->insert(*itr);
		addAllDependenciesForVertexIn(*itr, VFD, G);
	}
} 

SearchTree::SearchTree( std::string DescriptorPath, int Processors ) {
	_graph = new CommGraph(DescriptorPath);
	std::vector<std::set<int>*>* VerticesFullDependency = new std::vector<std::set<int>*>();
	for(int i=0; i<_graph->getQuantityOfVertices(); ++i)
		VerticesFullDependency->push_back(new std::set<int>());
	for(int i=0; i<_graph->getQuantityOfVertices(); ++i) {
		addAllDependenciesForVertexIn(i, VerticesFullDependency->at(i), _graph);
	}
	std::vector<std::set<int>*>* VerticesFullExpansion = new std::vector<std::set<int>*>();















	_numberOfNodes = 0;
	_numberOfProcessors = Processors;
	_numberOfTasks = _graph->getQuantityOfVertices();
	_answer = NULL;
	_root = new STNode(this);
	_expansionList.push_front(_root);
	_ordExpansionList.push(_root);
}
SearchTree::~SearchTree() {}
/*
int SearchTree::getNumberOfProcessors() {
	return _numberOfProcessors;
}
int SearchTree::getNumberOfTasks() {
	return _numberOfTasks;
}
SearchTree::CommGraph* SearchTree::getGraph() {
	return _graph;
}
SearchTree::AnswerType* SearchTree::getAnswer() {
	return _answer;
}

std::priority_queue<SearchTree::STNode*, std::deque<SearchTree::STNode*>, SearchTree::ComparisonContext>* SearchTree::getOrderedExpansionList() {
	return &_ordExpansionList;
}
void SearchTree::setAnswer( SearchTree::AnswerType* New_Answer ) {
	if(_answer != NULL)
		delete _answer;
	_answer = New_Answer;
}
void SearchTree::depthExpansion() {
	STNode* current;
	while(!_expansionList.empty()) {
		current = _expansionList.front();
		_expansionList.pop_front();
		current->depthExpansion();
	}
}
void SearchTree::breadthExpansion() {
	STNode* current;
	while(!_expansionList.empty()) {
		current = _expansionList.front();
		_expansionList.pop_front();
		current->breadthExpansion();
	}
}
void SearchTree::bestFirstExpansion() {
	STNode* current;
	while(!_ordExpansionList.empty()) {
		current = _ordExpansionList.top();
		_ordExpansionList.pop();
		current->bestFirstExpansion();
	}
}
void SearchTree::pushFront( SearchTree::STNode* Element ) {
	_expansionList.push_front(Element);
}
void SearchTree::pushBack( SearchTree::STNode* Element ) {
	_expansionList.push_back(Element);
}
void SearchTree::pushOrdered( SearchTree::STNode* Element ) {
	_ordExpansionList.push(Element);
}
void SearchTree::answerTreatment( SearchTree::STNode* Answer ) {
	if(_answer != NULL) {
		//if(Answer->getComparisonValue() >= _answer->getValue())
		//	return;
		_answer->~AnswerType();
		setAnswer(new AnswerType(Answer));
	}
	else
		setAnswer(new AnswerType(Answer));
}
void SearchTree::increaseNumberOfNodes() {
	++_numberOfNodes;
}
long long SearchTree::getNumberOfNodes() {
	return _numberOfNodes;
}

SearchTree::AnswerType::AnswerType( SearchTree::STNode *N ) {
	const int NumberOfProcs = N->getMainTree()->getNumberOfProcessors();
	for(int i=0; i<NumberOfProcs; ++i)
		_schedule.push_back(new std::deque<int>());
	SearchTree::STNode* ptrSTNode = N;
	while(ptrSTNode->getFatherNode() != NULL) {
		_schedule[ptrSTNode->getProcessor()]->push_front(ptrSTNode->getTask());
		ptrSTNode = ptrSTNode->getFatherNode();
	}
	_timming = N->getComparisonValue();
	for(int i=0; i<NumberOfProcs; ++i)
		_processorsStatus.push_back(N->getStatusForProcessor(i));
}
SearchTree::AnswerType::~AnswerType() {
}
void SearchTree::AnswerType::Print() {
	if(this != NULL) {
		for(unsigned int Proc=0; Proc<_schedule.size(); ++Proc) {
			std::cout << "\t" << "[";
			for(unsigned int i=0; i<_schedule[Proc]->size(); ++i)
				std::cout << " " << _schedule[Proc]->at(i) << " |";
			std::cout << "|]" << std::endl;
		}
	}
}
int SearchTree::AnswerType::getValue() {
	return _timming;
}
#endif  SEARCH_TREE_H 
*/
}
