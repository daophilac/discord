package com.peanut.discord.models;

public class User {
    private int userId;
    private String email;
    private String password;
    private String userName;
    private String fistName;
    private String lastName;
    private Gender gender;
    private String image;

    public int getUserId() {
        return userId;
    }

    public void setUserId(int userId) {
        this.userId = userId;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getFirstName() {
        return fistName;
    }

    public void setFirstName(String firstName) {
        fistName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public Gender getGender() {
        return gender;
    }

    public void setGender(Gender gender) {
        this.gender = gender;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }
    public enum Gender{
        Male(0), Female(1);
        private final int value;
        Gender(int value){
            this.value = value;
        }
        public int getValue() {
            return value;
        }
    }
}
