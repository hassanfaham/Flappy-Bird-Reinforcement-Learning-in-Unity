using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Unity.MLAgents.Demonstrations;
using UnityEngine;


public class DemoExtractor : MonoBehaviour
{
    private string demoFilePath;
    private FileSystem m_FileSystem;
    private DemonstrationWriter m_DemoWriter;

    void Start()
    {


        // Load the .demo file
        // var demoFilePath = "F:\\Unity_env\\ml-agents-release_20\\demos\\flappybirddemo_khalil_57.demo";
        // var demoFileStream = new FileStream(demoFilePath, FileMode.Open);
        // var demonstrationWriter = new DemonstrationWriter(demoFileStream);

        
        // Debug.Log(demonstrationWriter);

        // string filePath = "F:\\Unity_env\\ml-agents-release_20\\demos\\flappybirddemo_khalil_57.demo";
        // byte[] bytes = File.ReadAllBytes(filePath);

        // var reader = new BinaryReader(new MemoryStream(bytes));

        
    }
}
        








// from unityagents import DemonstrationWriter

// # Create a DemonstrationWriter object for reading the .demo file
// demo_file_path = "/path/to/demo/file.demo"
// demo_writer = DemonstrationWriter(demo_file_path)

// # Get the number of steps in the demonstration
// num_steps = demo_writer.get_num_steps()

// # Iterate over each step in the demonstration and extract the observation and action vectors
// for step_index in range(num_steps):
//     # Get the observation vector for the current step
//     observation = demo_writer.get_step_data(step_index)[0].obs

//     # Get the action vector for the current step
//     action = demo_writer.get_step_data(step_index)[0].action
