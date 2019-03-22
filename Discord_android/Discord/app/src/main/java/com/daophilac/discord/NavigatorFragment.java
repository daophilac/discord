package com.daophilac.discord;

import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.ColorFilter;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;

public class NavigatorFragment extends Fragment {

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_navigator, container, false);
//        ImageButton imageButton = view.findViewById(R.id.image_button);
//        String path = "/sdcard/server_1.png";
//        Bitmap bitmap = BitmapFactory.decodeFile(path);
//        imageButton.setBackground(new BitmapDrawable(getResources(), bitmap));

        String path = "/sdcard/server_1.png";
        Bitmap bitmap = BitmapFactory.decodeFile(path);
        Button button = view.findViewById(R.id.button);
        button.setBackground(new BitmapDrawable(getResources(), bitmap));
        return view;
    }

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Override
    public void onResume() {
        super.onResume();
    }
}
