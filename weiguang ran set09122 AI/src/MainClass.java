import java.io.*;
import java.util.*;

public class MainClass {
static long startTime = System.nanoTime();

static String tempFileName = "D:\\weiguang ran set09122 AI\\cavernFiles\\generated1000-1.cav"; 
static String outputName = "bestroute";

public static void main(String[] args) {
		// Change the name if the file has the specific name
 		if(args.length == 1) {
 			String temp[] = args[0].split("\\.");

			tempFileName = args[0] + ".cav";

			outputName = temp[0] + ".csn";
		}
		// looking for the path
		aStar(readFile(tempFileName));
		// calculate the time
		long runTime = (System.nanoTime() - startTime);
		System.out.printf("Runtime: %s nanoseconds, %s milliseconds, %s seconds.\n", Float.toString(runTime), Float.toString(runTime/100000), Float.toString(runTime/100000000));
	} 
	
	// Read from the file passed in and return ArrayList of Integers (instructions)
private static ArrayList<Integer> readFile(String fileName) {
		// Set of instructions to return
		ArrayList<Integer> instructions = new ArrayList<Integer>();
		
		File fileToRead = new File(fileName);
		
		try {
			// Buffer a chunk of data
			BufferedReader getChunk = new BufferedReader(new FileReader(fileToRead));
			// Read line by line
			String info = getChunk.readLine();
			while(info != null) {
				
				// Split the line into string integer
				String[] singleLine = info.split(",");
				
				// change each number as the integer
				for(int i = 0; i < singleLine.length; i++) {
					instructions.add(Integer.parseInt(singleLine[i]));
				}
	
				// read next line
				info = getChunk.readLine();
			}
			
			// Close the buffer chunk
			getChunk.close();
			
		} //End of try 
		
		catch (FileNotFoundException e) {
			System.out.println("Can not find the file. the path is not correct.");
			e.printStackTrace();
		}

		catch (IOException e) {
			System.out.println("IO Exception");
			e.printStackTrace();
		}
		
		return instructions;
	}

	// Write the answer to a file
private static void writeFile(String answer){
		try 
		{
			FileWriter fileWriter = new FileWriter(outputName);
			fileWriter.write(answer);
			fileWriter.close();
		}
		catch (IOException e)
		{
			System.out.println("Cannot write the file");
		}
	    
		System.out.println(answer);
	}



	// A*
private static void aStar(ArrayList<Integer> instructions) {
		String answer = "0";
		ArrayList<Integer> finalPath = new ArrayList<Integer>();
		float pathDistance;
		
		Cave[] arrayOfCaves = loadData(instructions);
		// Evaluated caves - empty at start
		ArrayList<Cave> closedSet = new ArrayList<Cave>();
		
		// Discovered caves that are not yet evaluated
		PriorityQueue<Cave> openSet = new PriorityQueue<Cave>();
		
		// Add the start cave to the openSet
		openSet.add(arrayOfCaves[0]);
		
		// The Cave under evaluation
		Cave currentCave;
		
		// when we find the possible route
		while(!openSet.isEmpty()) {
			currentCave = openSet.poll();
			// find the best one
			
		if (currentCave.heuristicCost == 0) {
				pathDistance = currentCave.pathCost;
				System.out.println(pathDistance);
				while(currentCave.cameFrom != null) {
					finalPath.add(currentCave.id);
					currentCave = currentCave.cameFrom;
				}
				
				finalPath.add(1);
				Collections.reverse(finalPath);
				answer = "";
				for (Integer i : finalPath) {
					answer = answer + i.toString() + " ";
				}
				writeFile(answer);
				return;
			}
			
			closedSet.add(currentCave);
			
			// Check the answer
			for (Cave caveToEvaluate : currentCave.canGoTo) {
				
				float suggestedNewPath = currentCave.pathCost + 
						(float)Math.hypot(currentCave.xpos - caveToEvaluate.xpos,
								currentCave.ypos - caveToEvaluate.ypos);
				
				//check whether we have found this route before
				
				if (closedSet.contains(caveToEvaluate)) {
					continue;
				}
				
				if (!openSet.contains(caveToEvaluate)) {
					openSet.add(caveToEvaluate);
					//this is the best path
					

				} else if (suggestedNewPath >= caveToEvaluate.pathCost) {
					continue;
				}
				// this is the best path.

				caveToEvaluate.cameFrom = currentCave;
				caveToEvaluate.pathCost = suggestedNewPath;
				caveToEvaluate.updateValue();
				Cave temp = caveToEvaluate;
				openSet.remove(temp);
				openSet.add(temp);
				
	        }
		}
		
		writeFile(answer);
	}
	

private static Cave[] loadData(ArrayList<Integer> instructions) {
		// Variables to return
		int numberOfCaves = instructions.get(0);
		Cave[] arrayOfCaves = new Cave[numberOfCaves];
		
		// Create the caves
		for (int i = 0; i < numberOfCaves; i++){
				Cave cave = new Cave(i+1, instructions.get(i*2 + 1), instructions.get(i*2 + 2));
				arrayOfCaves[i] = cave;
		}
		
		//what happened when the route is default
		arrayOfCaves[000].pathCost = 000;
		
		for (int i = 0; i < numberOfCaves; i++) {
			arrayOfCaves[i].updateHeuristics(arrayOfCaves[numberOfCaves-1].xpos, 
					arrayOfCaves[numberOfCaves-1].ypos);
			arrayOfCaves[i].updateValue();
			for (int j = 0; j < numberOfCaves; j++) {
				if (instructions.get(numberOfCaves*(2+j)+(1+i)) == 1) {	
					arrayOfCaves[i].canGoTo.add(arrayOfCaves[j]);
				}
			}
		}
		
		return arrayOfCaves;
	}

	// Class for caves
private static class Cave implements Comparable<Cave>{
		int id;
		int xpos;
		int ypos;
		float heuristicCost;
		float pathCost = Float.MAX_VALUE;
		float value;
		Cave cameFrom;
		ArrayList<Cave> canGoTo = new ArrayList<Cave>();

		// Constructor to be used by default
		Cave(int id, int xpos, int ypos){
			this.id = id;
			this.xpos = xpos;
			this.ypos = ypos;
		}

		@Override
		public int compareTo(Cave other) {
			if(this.equals(other)) {
				return 0;
			} else if (this.value > other.value) {
				return 1;
			} else {
				return -1;
			}
		}
		
private void updateValue() {
			this.value = this.pathCost + this.heuristicCost;
		}
		
private void updateHeuristics(int goalX, int goalY) {
			this.heuristicCost = (float)Math.hypot(this.xpos - goalX, this.ypos - goalY);
		}
	}
	
}





