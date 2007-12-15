xof 0303txt 0032


template VertexDuplicationIndices { 
 <b8d65549-d7c9-4995-89cf-53a9a8b031e3>
 DWORD nIndices;
 DWORD nOriginalVertices;
 array DWORD indices[nIndices];
}
template XSkinMeshHeader {
 <3cf169ce-ff7c-44ab-93c0-f78f62d172e2>
 WORD nMaxSkinWeightsPerVertex;
 WORD nMaxSkinWeightsPerFace;
 WORD nBones;
}
template SkinWeights {
 <6f0d123b-bad2-4167-a0d0-80224f25fabb>
 STRING transformNodeName;
 DWORD nWeights;
 array DWORD vertexIndices[nWeights];
 array float weights[nWeights];
 Matrix4x4 matrixOffset;
}

Frame RootFrame {

  FrameTransformMatrix {
    1.000000,0.000000,0.000000,0.000000,
    0.000000,0.000000,-1.000000,0.000000,
    0.000000,1.000000,0.000000,0.000000,
    0.000000,0.000000,0.000000,1.000000;;
  }
  Frame pCube1_initialShading {

    FrameTransformMatrix {
      0.100000,0.000000,0.000000,0.000000,
      0.000000,0.100000,0.000000,0.000000,
      0.000000,0.000000,0.100000,0.000000,
      0.000000,0.000000,0.000000,1.000000;;
    }
Mesh {
152;
-2.044100; -3.500000; 0.000000;,
2.044100; -3.500000; 0.000000;,
2.044100; -3.500000; 0.005700;,
-2.044100; -3.500000; 0.005700;,
-7.021200; -3.590000; 10.201700;,
7.021200; -3.590000; 10.201700;,
7.021200; 3.431200; 10.201700;,
-7.021200; 3.431200; 10.201700;,
-7.000000; 3.500000; 1.022000;,
7.000000; 3.500000; 1.022000;,
7.000000; 3.500000; 0.000000;,
-7.000000; 3.500000; 0.000000;,
-7.000000; 3.500000; 0.000000;,
7.000000; 3.500000; 0.000000;,
7.000000; -3.500000; 0.000000;,
-7.000000; -3.500000; 0.000000;,
7.000000; -3.500000; 0.000000;,
7.000000; 3.500000; 0.000000;,
7.000000; 3.500000; 1.022000;,
7.000000; -3.500000; 1.022000;,
-7.000000; 3.500000; 0.000000;,
-7.000000; -3.500000; 0.000000;,
-7.000000; -3.500000; 1.022000;,
-7.000000; 3.500000; 1.022000;,
-2.044100; -3.500000; 0.005700;,
2.044100; -3.500000; 0.005700;,
1.392500; -1.581100; 1.022000;,
-1.392500; -1.581100; 1.022000;,
7.000000; -3.500000; 1.022000;,
7.000000; 3.500000; 1.022000;,
4.768500; 3.187400; 1.022000;,
4.768500; -1.581100; 1.022000;,
7.000000; 3.500000; 1.022000;,
-7.000000; 3.500000; 1.022000;,
-4.768500; 3.187400; 1.022000;,
4.768500; 3.187400; 1.022000;,
-7.000000; 3.500000; 1.022000;,
-7.000000; -3.500000; 1.022000;,
-4.768500; -1.581100; 1.022000;,
-4.768500; 3.187400; 1.022000;,
4.768500; -1.581100; 1.022000;,
4.768500; 3.187400; 1.022000;,
4.768500; 3.187400; 9.258500;,
4.768500; -1.581100; 9.258500;,
-4.768500; 3.187400; 1.022000;,
-4.768500; -1.581100; 1.022000;,
-4.768500; -1.581100; 9.258500;,
-4.768500; 3.187400; 9.258500;,
-4.768500; -1.581100; 9.258500;,
4.768500; -1.581100; 9.258500;,
7.021200; -3.590000; 9.258500;,
-7.021200; -3.590000; 9.258500;,
4.768500; -1.581100; 9.258500;,
4.768500; 3.187400; 9.258500;,
7.021200; 3.431200; 9.258500;,
7.021200; -3.590000; 9.258500;,
4.768500; 3.187400; 9.258500;,
-4.768500; 3.187400; 9.258500;,
-7.021200; 3.431200; 9.258500;,
7.021200; 3.431200; 9.258500;,
-4.768500; 3.187400; 9.258500;,
-4.768500; -1.581100; 9.258500;,
-7.021200; -3.590000; 9.258500;,
-7.021200; 3.431200; 9.258500;,
-7.021200; -3.590000; 9.258500;,
7.021200; -3.590000; 9.258500;,
7.021200; -3.590000; 10.201700;,
-7.021200; -3.590000; 10.201700;,
7.021200; -3.590000; 9.258500;,
7.021200; 3.431200; 9.258500;,
7.021200; 3.431200; 10.201700;,
7.021200; -3.590000; 10.201700;,
7.021200; 3.431200; 9.258500;,
-7.021200; 3.431200; 9.258500;,
-7.021200; 3.431200; 10.201700;,
7.021200; 3.431200; 10.201700;,
-7.021200; 3.431200; 9.258500;,
-7.021200; -3.590000; 9.258500;,
-7.021200; -3.590000; 10.201700;,
-7.021200; 3.431200; 10.201700;,
2.044100; -3.500000; 0.000000;,
-2.044100; -3.500000; 0.000000;,
-7.000000; -3.500000; 0.000000;,
7.000000; -3.500000; 0.000000;,
7.000000; -3.500000; 0.000000;,
7.000000; -3.500000; 1.022000;,
2.044100; -3.500000; 0.005700;,
2.044100; -3.500000; 0.000000;,
-2.044100; -3.500000; 0.000000;,
-2.044100; -3.500000; 0.005700;,
-7.000000; -3.500000; 1.022000;,
-7.000000; -3.500000; 0.000000;,
7.000000; -3.500000; 1.022000;,
4.768500; -1.581100; 1.022000;,
1.392500; -1.581100; 1.022000;,
2.044100; -3.500000; 0.005700;,
4.768500; -1.581100; 1.022000;,
-4.768500; -1.581100; 1.022000;,
-1.392500; -1.581100; 1.022000;,
1.392500; -1.581100; 1.022000;,
-4.768500; -1.581100; 1.022000;,
-7.000000; -3.500000; 1.022000;,
-2.044100; -3.500000; 0.005700;,
-1.392500; -1.581100; 1.022000;,
-4.768500; -1.581100; 1.022000;,
4.768500; -1.581100; 1.022000;,
3.814900; -0.141000; 1.070000;,
-3.814900; -0.141000; 1.070000;,
4.768500; -1.581100; 1.022000;,
4.768500; -1.581100; 9.258500;,
3.814900; -0.141000; 8.417200;,
3.814900; -0.141000; 1.070000;,
4.768500; -1.581100; 9.258500;,
-4.768500; -1.581100; 9.258500;,
-3.814900; -0.141000; 8.417200;,
3.814900; -0.141000; 8.417200;,
-4.768500; -1.581100; 9.258500;,
-4.768500; -1.581100; 1.022000;,
-3.814900; -0.141000; 1.070000;,
-3.814900; -0.141000; 8.417200;,
-3.814900; -0.141000; 1.070000;,
3.814900; -0.141000; 1.070000;,
3.814900; 3.178100; 1.070000;,
-3.814900; 3.178100; 1.070000;,
3.814900; -0.141000; 1.070000;,
3.814900; -0.141000; 8.417200;,
3.814900; 3.178100; 8.417200;,
3.814900; 3.178100; 1.070000;,
3.814900; -0.141000; 8.417200;,
-3.814900; -0.141000; 8.417200;,
-3.814900; 3.178100; 8.417200;,
3.814900; 3.178100; 8.417200;,
-3.814900; -0.141000; 8.417200;,
-3.814900; -0.141000; 1.070000;,
-3.814900; 3.178100; 1.070000;,
-3.814900; 3.178100; 8.417200;,
-3.814900; 3.178100; 1.070000;,
3.814900; 3.178100; 1.070000;,
4.792500; 3.178100; 0.128500;,
-4.792500; 3.178100; 0.128500;,
3.814900; 3.178100; 1.070000;,
3.814900; 3.178100; 8.417200;,
4.792500; 3.178100; 9.358700;,
4.792500; 3.178100; 0.128500;,
3.814900; 3.178100; 8.417200;,
-3.814900; 3.178100; 8.417200;,
-4.792500; 3.178100; 9.358700;,
4.792500; 3.178100; 9.358700;,
-3.814900; 3.178100; 8.417200;,
-3.814900; 3.178100; 1.070000;,
-4.792500; 3.178100; 0.128500;,
-4.792500; 3.178100; 9.358700;;
38;
4; 0, 1, 2, 3;,
4; 4, 5, 6, 7;,
4; 8, 9, 10, 11;,
4; 12, 13, 14, 15;,
4; 16, 17, 18, 19;,
4; 20, 21, 22, 23;,
4; 24, 25, 26, 27;,
4; 28, 29, 30, 31;,
4; 32, 33, 34, 35;,
4; 36, 37, 38, 39;,
4; 40, 41, 42, 43;,
4; 44, 45, 46, 47;,
4; 48, 49, 50, 51;,
4; 52, 53, 54, 55;,
4; 56, 57, 58, 59;,
4; 60, 61, 62, 63;,
4; 64, 65, 66, 67;,
4; 68, 69, 70, 71;,
4; 72, 73, 74, 75;,
4; 76, 77, 78, 79;,
4; 80, 81, 82, 83;,
4; 84, 85, 86, 87;,
4; 88, 89, 90, 91;,
4; 92, 93, 94, 95;,
4; 96, 97, 98, 99;,
4; 100, 101, 102, 103;,
4; 104, 105, 106, 107;,
4; 108, 109, 110, 111;,
4; 112, 113, 114, 115;,
4; 116, 117, 118, 119;,
4; 120, 121, 122, 123;,
4; 124, 125, 126, 127;,
4; 128, 129, 130, 131;,
4; 132, 133, 134, 135;,
4; 136, 137, 138, 139;,
4; 140, 141, 142, 143;,
4; 144, 145, 146, 147;,
4; 148, 149, 150, 151;;
  MeshMaterialList {
    1;
    38;
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0;;
  Material initialShadingGroup {
    0.800000; 0.800000; 0.800000;1.0;;
    0.500000;
    1.000000; 1.000000; 1.000000;;
    0.0; 0.0; 0.0;;
  }  //End of Material
    }  //End of MeshMaterialList
  MeshNormals {
152;
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    -0.034120; -0.838160; 0.544328;,
    0.034120; -0.838160; 0.544328;,
    -0.577349; -0.577349; 0.577349;,
    0.577349; -0.577349; 0.577349;,
    0.577349; 0.577349; 0.577349;,
    -0.577349; 0.577349; 0.577349;,
    -0.408246; 0.408246; 0.816492;,
    0.408246; 0.408246; 0.816492;,
    0.577349; 0.577349; -0.577349;,
    -0.577349; 0.577349; -0.577349;,
    -0.577349; 0.577349; -0.577349;,
    0.577349; 0.577349; -0.577349;,
    0.577349; -0.577349; -0.577349;,
    -0.577349; -0.577349; -0.577349;,
    0.577349; -0.577349; -0.577349;,
    0.577349; 0.577349; -0.577349;,
    0.408246; 0.408246; 0.816492;,
    0.352641; -0.530839; 0.770592;,
    -0.577349; 0.577349; -0.577349;,
    -0.577349; -0.577349; -0.577349;,
    -0.352641; -0.530839; 0.770592;,
    -0.408246; 0.408246; 0.816492;,
    0.034120; -0.838160; 0.544328;,
    -0.034120; -0.838160; 0.544328;,
    -0.057314; -0.402478; 0.913602;,
    0.057314; -0.402478; 0.913602;,
    0.352641; -0.530839; 0.770592;,
    0.408246; 0.408246; 0.816492;,
    0.447188; 0.000000; 0.894406;,
    0.016907; -0.298715; 0.954161;,
    0.408246; 0.408246; 0.816492;,
    -0.408246; 0.408246; 0.816492;,
    -0.447188; 0.000000; 0.894406;,
    0.447188; 0.000000; 0.894406;,
    -0.408246; 0.408246; 0.816492;,
    -0.352641; -0.530839; 0.770592;,
    -0.016907; -0.298715; 0.954161;,
    -0.447188; 0.000000; 0.894406;,
    0.016907; -0.298715; 0.954161;,
    0.447188; 0.000000; 0.894406;,
    0.447188; 0.000000; -0.894406;,
    0.054384; -0.345622; -0.936766;,
    -0.447188; 0.000000; 0.894406;,
    -0.016907; -0.298715; 0.954161;,
    -0.054384; -0.345622; -0.936766;,
    -0.447188; 0.000000; -0.894406;,
    -0.054384; -0.345622; -0.936766;,
    0.054384; -0.345622; -0.936766;,
    0.408246; -0.408246; -0.816492;,
    -0.408246; -0.408246; -0.816492;,
    0.054384; -0.345622; -0.936766;,
    0.447188; 0.000000; -0.894406;,
    0.408246; 0.408246; -0.816492;,
    0.408246; -0.408246; -0.816492;,
    0.447188; 0.000000; -0.894406;,
    -0.447188; 0.000000; -0.894406;,
    -0.408246; 0.408246; -0.816492;,
    0.408246; 0.408246; -0.816492;,
    -0.447188; 0.000000; -0.894406;,
    -0.054384; -0.345622; -0.936766;,
    -0.408246; -0.408246; -0.816492;,
    -0.408246; 0.408246; -0.816492;,
    -0.408246; -0.408246; -0.816492;,
    0.408246; -0.408246; -0.816492;,
    0.577349; -0.577349; 0.577349;,
    -0.577349; -0.577349; 0.577349;,
    0.408246; -0.408246; -0.816492;,
    0.408246; 0.408246; -0.816492;,
    0.577349; 0.577349; 0.577349;,
    0.577349; -0.577349; 0.577349;,
    0.408246; 0.408246; -0.816492;,
    -0.408246; 0.408246; -0.816492;,
    -0.577349; 0.577349; 0.577349;,
    0.577349; 0.577349; 0.577349;,
    -0.408246; 0.408246; -0.816492;,
    -0.408246; -0.408246; -0.816492;,
    -0.577349; -0.577349; 0.577349;,
    -0.577349; 0.577349; 0.577349;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    -0.577349; -0.577349; -0.577349;,
    0.577349; -0.577349; -0.577349;,
    0.577349; -0.577349; -0.577349;,
    0.352641; -0.530839; 0.770592;,
    -0.034120; -0.838160; 0.544328;,
    0.000000; -1.000000; 0.000000;,
    0.000000; -1.000000; 0.000000;,
    0.034120; -0.838160; 0.544328;,
    -0.352641; -0.530839; 0.770592;,
    -0.577349; -0.577349; -0.577349;,
    0.352641; -0.530839; 0.770592;,
    0.016907; -0.298715; 0.954161;,
    -0.057314; -0.402478; 0.913602;,
    -0.034120; -0.838160; 0.544328;,
    0.016907; -0.298715; 0.954161;,
    -0.016907; -0.298715; 0.954161;,
    0.057314; -0.402478; 0.913602;,
    -0.057314; -0.402478; 0.913602;,
    -0.016907; -0.298715; 0.954161;,
    -0.352641; -0.530839; 0.770592;,
    0.034120; -0.838160; 0.544328;,
    0.057314; -0.402478; 0.913602;,
    -0.016907; -0.298715; 0.954161;,
    0.016907; -0.298715; 0.954161;,
    -0.660695; -0.210913; 0.720389;,
    0.660695; -0.210913; 0.720389;,
    0.016907; -0.298715; 0.954161;,
    0.054384; -0.345622; -0.936766;,
    -0.650288; -0.374676; -0.660817;,
    -0.660695; -0.210913; 0.720389;,
    0.054384; -0.345622; -0.936766;,
    -0.054384; -0.345622; -0.936766;,
    0.650288; -0.374676; -0.660817;,
    -0.650288; -0.374676; -0.660817;,
    -0.054384; -0.345622; -0.936766;,
    -0.016907; -0.298715; 0.954161;,
    0.660695; -0.210913; 0.720389;,
    0.650288; -0.374676; -0.660817;,
    0.660695; -0.210913; 0.720389;,
    -0.660695; -0.210913; 0.720389;,
    -0.408246; 0.816492; 0.408246;,
    0.408246; 0.816492; 0.408246;,
    -0.660695; -0.210913; 0.720389;,
    -0.650288; -0.374676; -0.660817;,
    -0.408246; 0.816492; -0.408246;,
    -0.408246; 0.816492; 0.408246;,
    -0.650288; -0.374676; -0.660817;,
    0.650288; -0.374676; -0.660817;,
    0.408246; 0.816492; -0.408246;,
    -0.408246; 0.816492; -0.408246;,
    0.650288; -0.374676; -0.660817;,
    0.660695; -0.210913; 0.720389;,
    0.408246; 0.816492; 0.408246;,
    0.408246; 0.816492; -0.408246;,
    0.408246; 0.816492; 0.408246;,
    -0.408246; 0.816492; 0.408246;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    -0.408246; 0.816492; 0.408246;,
    -0.408246; 0.816492; -0.408246;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    -0.408246; 0.816492; -0.408246;,
    0.408246; 0.816492; -0.408246;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;,
    0.408246; 0.816492; -0.408246;,
    0.408246; 0.816492; 0.408246;,
    0.000000; 1.000000; 0.000000;,
    0.000000; 1.000000; 0.000000;;
38;
4; 0, 1, 2, 3;,
4; 4, 5, 6, 7;,
4; 8, 9, 10, 11;,
4; 12, 13, 14, 15;,
4; 16, 17, 18, 19;,
4; 20, 21, 22, 23;,
4; 24, 25, 26, 27;,
4; 28, 29, 30, 31;,
4; 32, 33, 34, 35;,
4; 36, 37, 38, 39;,
4; 40, 41, 42, 43;,
4; 44, 45, 46, 47;,
4; 48, 49, 50, 51;,
4; 52, 53, 54, 55;,
4; 56, 57, 58, 59;,
4; 60, 61, 62, 63;,
4; 64, 65, 66, 67;,
4; 68, 69, 70, 71;,
4; 72, 73, 74, 75;,
4; 76, 77, 78, 79;,
4; 80, 81, 82, 83;,
4; 84, 85, 86, 87;,
4; 88, 89, 90, 91;,
4; 92, 93, 94, 95;,
4; 96, 97, 98, 99;,
4; 100, 101, 102, 103;,
4; 104, 105, 106, 107;,
4; 108, 109, 110, 111;,
4; 112, 113, 114, 115;,
4; 116, 117, 118, 119;,
4; 120, 121, 122, 123;,
4; 124, 125, 126, 127;,
4; 128, 129, 130, 131;,
4; 132, 133, 134, 135;,
4; 136, 137, 138, 139;,
4; 140, 141, 142, 143;,
4; 144, 145, 146, 147;,
4; 148, 149, 150, 151;;
}  //End of MeshNormals
MeshTextureCoords {
152;
0.375000;-0.000000;,
0.625000;-0.000000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.500000;,
0.625000;-0.500000;,
0.625000;-0.750000;,
0.375000;-0.750000;,
0.375000;-0.750000;,
0.625000;-0.750000;,
0.625000;-1.000000;,
0.375000;-1.000000;,
0.625000;-0.000000;,
0.875000;-0.000000;,
0.875000;-0.250000;,
0.625000;-0.250000;,
0.125000;-0.000000;,
0.375000;-0.000000;,
0.375000;-0.250000;,
0.125000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.625000;-0.500000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.500000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.500000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.625000;-0.500000;,
0.625000;-0.250000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.625000;-0.500000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.500000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.625000;-0.500000;,
0.625000;-0.250000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.500000;,
0.625000;-0.500000;,
0.375000;-0.500000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.500000;,
0.625000;-0.000000;,
0.375000;-0.000000;,
0.375000;-0.000000;,
0.625000;-0.000000;,
0.625000;-0.000000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.000000;,
0.375000;-0.000000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.000000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.625000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;,
0.375000;-0.250000;;
}  //End of MeshTextureCoords
  }  // End of the Mesh pCube1_initialShading 
  }  // SI End of the Object pCube1_initialShading 
}  // End of the Root Frame
