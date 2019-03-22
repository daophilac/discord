package com.daophilac.customview;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class MainActivity extends AppCompatActivity {
    private FlexibleButton flexibleButton;
    private Button buttonWidth;
    private Button buttonHeight;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        this.flexibleButton = findViewById(R.id.myButton);
        this.buttonWidth = findViewById(R.id.buttonWidth);
        this.buttonHeight = findViewById(R.id.buttonHeight);
        this.buttonWidth.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                increaseWidth();
            }
        });
        this.buttonHeight.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                increaseHeight();
            }
        });
//        setContentView(R.layout.activity_main);

        //myCustomView = (MyCustomView) findViewById(R.id.mcv);
    }
    protected void increaseWidth(){
        this.flexibleButton.increaseWidth(20);
    }
    protected void increaseHeight(){
        this.flexibleButton.increaseHeight(20);
    }

//    public void onClick(View view) {
//        switch (view.getId()){
//            case R.id.one:{
//                myCustomView.customPaddingUp(30);
//                break;
//            }
//            case R.id.two:{
//                myCustomView.swapColor();
//                break;
//            }
//            case R.id.three:{
//                myCustomView.customPaddingDown(30);
//            }
//        }
//    }
}